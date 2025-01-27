using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAttendanceRecordsAsync(
            DateTime startDate,
            DateTime endDate,
            int? departmentId = null,
            int? subjectId = null,
            int? studyGroupId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<AttendanceRecord>()
                .Include(ar => ar.CourseSession!)
                    .ThenInclude(c => c.StudyGroup!)
                        .ThenInclude(sg => sg.Subject!)
                            .ThenInclude(s => s.Department)
                .Where(ar => ar.CheckInTime >= startDate && ar.CheckInTime <= endDate)
                .AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(ar => ar.CourseSession != null && 
                                        ar.CourseSession.StudyGroup != null && 
                                        ar.CourseSession.StudyGroup.Subject != null && 
                                        ar.CourseSession.StudyGroup.Subject.DepartmentId == departmentId.Value);
            }

            if (subjectId.HasValue)
            {
                query = query.Where(ar => ar.CourseSession != null && 
                                        ar.CourseSession.StudyGroup != null && 
                                        ar.CourseSession.StudyGroup.SubjectId == subjectId.Value);
            }

            if (studyGroupId.HasValue)
            {
                query = query.Where(ar => ar.CourseSession != null && 
                                        ar.CourseSession.StudyGroupId == studyGroupId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Student>> GetStudentsWithHighAbsenceAsync(
            double absenceThreshold,
            CancellationToken cancellationToken = default)
        {
            var studentsWithHighAbsence = new List<Student>();
            var activeStudyGroups = await _context.Set<StudyGroup>()
                .Where(sg => sg.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var studyGroup in activeStudyGroups)
            {
                var enrolledStudents = await _context.Set<GroupStudent>()
                    .Include(gs => gs.Student!)
                        .ThenInclude(s => s.User)
                    .Where(gs => gs.StudyGroupId == studyGroup.Id && gs.Student != null)
                    .ToListAsync(cancellationToken);

                foreach (var enrollment in enrolledStudents)
                {
                    if (enrollment.Student == null) continue;
                    
                    var attendancePercentage = await CalculateStudentAttendancePercentage(
                        enrollment.StudentId,
                        studyGroup.Id,
                        cancellationToken);

                    if ((100 - attendancePercentage) >= absenceThreshold &&
                        !studentsWithHighAbsence.Any(s => s.Id == enrollment.StudentId))
                    {
                        studentsWithHighAbsence.Add(enrollment.Student);
                    }
                }
            }

            return studentsWithHighAbsence;
        }

        private async Task<double> CalculateStudentAttendancePercentage(
            int studentId,
            int studyGroupId,
            CancellationToken cancellationToken)
        {
            var totalCourseSessions = await _context.Set<CourseSession>()
                .CountAsync(c => c.StudyGroupId == studyGroupId && c.IsActive, cancellationToken);

            if (totalCourseSessions == 0)
            {
                return 100; // No Course Sessions held yet
            }

            var attendedCourseSessions = await _context.Set<AttendanceRecord>()
                .CountAsync(ar =>
                    ar.StudentId == studentId &&
                    ar.CourseSession != null &&
                    ar.CourseSession.StudyGroupId == studyGroupId &&
                    ar.IsConfirmed,
                    cancellationToken);

            return (double)attendedCourseSessions / totalCourseSessions * 100;
        }
    }
}