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
                .Include(ar => ar.Course!)
                    .ThenInclude(c => c.StudyGroup!)
                        .ThenInclude(sg => sg.Subject!)
                            .ThenInclude(s => s.Department)
                .Where(ar => ar.CheckInTime >= startDate && ar.CheckInTime <= endDate)
                .AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(ar => ar.Course != null && 
                                        ar.Course.StudyGroup != null && 
                                        ar.Course.StudyGroup.Subject != null && 
                                        ar.Course.StudyGroup.Subject.DepartmentId == departmentId.Value);
            }

            if (subjectId.HasValue)
            {
                query = query.Where(ar => ar.Course != null && 
                                        ar.Course.StudyGroup != null && 
                                        ar.Course.StudyGroup.SubjectId == subjectId.Value);
            }

            if (studyGroupId.HasValue)
            {
                query = query.Where(ar => ar.Course != null && 
                                        ar.Course.StudyGroupId == studyGroupId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Student>> GetStudentsWithHighAbsenceAsync(
            double absenceThreshold,
            CancellationToken cancellationToken = default)
        {
            var studentsWithHighAbsence = new List<Student>();
            var activeGroups = await _context.Set<StudyGroup>()
                .Where(sg => sg.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var group in activeGroups)
            {
                var enrolledStudents = await _context.Set<GroupStudent>()
                    .Include(gs => gs.Student!)
                        .ThenInclude(s => s.User)
                    .Where(gs => gs.StudyGroupId == group.Id && gs.Student != null)
                    .ToListAsync(cancellationToken);

                foreach (var enrollment in enrolledStudents)
                {
                    if (enrollment.Student == null) continue;
                    
                    var attendancePercentage = await CalculateStudentAttendancePercentage(
                        enrollment.StudentId,
                        group.Id,
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
            var totalClasses = await _context.Set<Course>()
                .CountAsync(c => c.StudyGroupId == studyGroupId && c.IsActive, cancellationToken);

            if (totalClasses == 0)
            {
                return 100; // No classes held yet
            }

            var attendedClasses = await _context.Set<AttendanceRecord>()
                .CountAsync(ar =>
                    ar.StudentId == studentId &&
                    ar.Course != null &&
                    ar.Course.StudyGroupId == studyGroupId &&
                    ar.IsConfirmed,
                    cancellationToken);

            return (double)attendedClasses / totalClasses * 100;
        }
    }
}