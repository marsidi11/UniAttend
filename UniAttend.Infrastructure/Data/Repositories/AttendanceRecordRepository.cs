using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Stats;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;
using UniAttend.Core.Enums;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing attendance records in the database.
    /// </summary>
    public class AttendanceRecordRepository : BaseRepository<AttendanceRecord>, IAttendanceRecordRepository
    {
        public AttendanceRecordRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves attendance records for a specific course session.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetByCourseSessionIdAsync(
            int courseSessionId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.CourseSessionId == courseSessionId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves attendance records for a specific student.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(
            int studentId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.StudentId == studentId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves attendance records within a date range.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.CheckInTime >= startDate && a.CheckInTime <= endDate)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Gets a student's attendance for a specific course session.
        /// </summary>
        public async Task<AttendanceRecord?> GetStudentAttendanceForSessionAsync(
            int studentId,
            int courseSessionId,
            CancellationToken cancellationToken = default)
            => await DbSet
                .FirstOrDefaultAsync(a =>
                    a.StudentId == studentId &&
                    a.CourseSessionId == courseSessionId,
                    cancellationToken);

        /// <summary>
        /// Calculates a student's attendance percentage for a study group.
        /// </summary>
        public async Task<double> GetStudentAttendancePercentageAsync(
            int studentId,
            int studyGroupId,
            CancellationToken cancellationToken)
        {
            var totalSessions = await GetConfirmedSessionCountAsync(studyGroupId, cancellationToken);
            if (totalSessions == 0) return 0;
            
            var attendedSessions = await GetStudentAttendedSessionsCountAsync(studentId, studyGroupId, cancellationToken);
            return ((double)attendedSessions / totalSessions) * 100;
        }

        /// <summary>
        /// Counts confirmed sessions in a study group.
        /// </summary>
        public async Task<int> GetConfirmedSessionCountAsync(
            int studyGroupId,
            CancellationToken cancellationToken)
            => await Context.Set<CourseSession>()
                .CountAsync(cs =>
                    cs.StudyGroupId == studyGroupId &&
                    cs.IsActive &&
                    cs.Status == SessionStatus.Confirmed,
                    cancellationToken);

        /// <summary>
        /// Counts confirmed sessions attended by a student.
        /// </summary>
        public async Task<int> GetStudentAttendedSessionsCountAsync(
            int studentId,
            int studyGroupId,
            CancellationToken cancellationToken)
            => await DbSet
                .CountAsync(a =>
                    a.StudentId == studentId &&
                    a.CourseSession.StudyGroupId == studyGroupId &&
                    a.IsConfirmed &&
                    !a.IsAbsent,
                    cancellationToken);

        /// <summary>
        /// Retrieves unconfirmed attendance records for a course session.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(
            int courseSessionId,
            CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a =>
                    a.CourseSessionId == courseSessionId &&
                    !a.IsConfirmed)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Confirms attendance records for a course session.
        /// </summary>
        public async Task ConfirmAttendanceRecordsAsync(
            int courseSessionId,
            CancellationToken cancellationToken = default)
        {
            var records = await GetUnconfirmedRecordsAsync(courseSessionId, cancellationToken);
            foreach (var record in records)
            {
                record.Confirm();
                DbSet.Update(record);
            }
            await Context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves attendance records for a study group within a date range.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetGroupAttendanceAsync(
            int studyGroupId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(ar => ar.Student)
                    .ThenInclude(s => s.User)
                .Include(ar => ar.CourseSession)
                    .ThenInclude(cs => cs.StudyGroup)
                .Where(ar =>
                    ar.CourseSession.StudyGroupId == studyGroupId &&
                    ar.CheckInTime >= startDate &&
                    ar.CheckInTime <= endDate)
                .OrderByDescending(ar => ar.CheckInTime)
                .ThenBy(ar => ar.Student.User.LastName)
                .ThenBy(ar => ar.Student.User.FirstName)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Gets a student's attendance for a specific course session.
        /// </summary>
        public async Task<AttendanceRecord?> GetStudentAttendanceForCourseSessionAsync(
            int studentId,
            int courseSessionId,
            CancellationToken cancellationToken = default)
            => await DbSet
                .FirstOrDefaultAsync(a =>
                    a.StudentId == studentId &&
                    a.CourseSessionId == courseSessionId,
                    cancellationToken);

        /// <summary>
        /// Gets a course session with full details.
        /// </summary>
        public async Task<CourseSession> GetSessionWithDetailsAsync(
            int sessionId,
            CancellationToken cancellationToken = default)
        {
            var session = await Context.Set<CourseSession>()
                .Include(cs => cs.StudyGroup)
                    .ThenInclude(g => g.Students)
                        .ThenInclude(gs => gs.Student)
                            .ThenInclude(s => s.User)
                .Include(cs => cs.Classroom)
                .FirstOrDefaultAsync(cs => cs.Id == sessionId, cancellationToken);

            if (session == null)
                throw new KeyNotFoundException($"Course session with ID {sessionId} not found");

            return session;
        }

        /// <summary>
        /// Retrieves a student's attendance summary for a study group.
        /// </summary>
        public async Task<(int TotalCourseSessions, int AttendedCourseSessions)> GetStudentGroupAttendanceAsync(
            int studentId,
            int studyGroupId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(ar => ar.CourseSession)
                .Where(ar =>
                    ar.StudentId == studentId &&
                    ar.CourseSession.StudyGroupId == studyGroupId);

            if (startDate.HasValue)
                query = query.Where(ar => ar.CheckInTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ar => ar.CheckInTime <= endDate.Value);

            var attendance = await query.ToListAsync(cancellationToken);

            var totalCourseSessions = await Context.Set<CourseSession>()
                .CountAsync(c =>
                    c.StudyGroupId == studyGroupId &&
                    (!startDate.HasValue || c.Date >= startDate.Value.Date) &&
                    (!endDate.HasValue || c.Date <= endDate.Value.Date),
                    cancellationToken);

            return (totalCourseSessions, attendance.Count(a => a.IsConfirmed));
        }

        /// <summary>
        /// Generates an academic year attendance report.
        /// </summary>
        public async Task<AttendanceReportResult> GetAcademicYearAttendanceReportAsync(
            int academicYearId,
            CancellationToken cancellationToken = default)
        {
            var records = await DbSet
                .Include(ar => ar.CourseSession)
                    .ThenInclude(c => c.StudyGroup)
                .Where(ar => ar.CourseSession.StudyGroup.AcademicYearId == academicYearId)
                .ToListAsync(cancellationToken);

            return new AttendanceReportResult
            {
                OverallAttendance = records.Any()
                    ? (decimal)records.Count(r => r.IsConfirmed) / records.Count * 100
                    : 0,
                PendingConfirmations = records.Count(r => !r.IsConfirmed),
                TotalRecords = records.Count
            };
        }

        /// <summary>
        /// Generates a study group attendance report.
        /// </summary>
        public async Task<AttendanceReportResult> GetGroupAttendanceReportAsync(
            int studyGroupId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(ar => ar.CourseSession)
                .Where(ar => ar.CourseSession.StudyGroupId == studyGroupId);

            if (startDate.HasValue)
                query = query.Where(ar => ar.CheckInTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ar => ar.CheckInTime <= endDate.Value);

            var records = await query.ToListAsync(cancellationToken);

            return new AttendanceReportResult
            {
                OverallAttendance = records.Any()
                    ? (decimal)records.Count(r => r.IsConfirmed) / records.Count * 100
                    : 0,
                PendingConfirmations = records.Count(r => !r.IsConfirmed),
                TotalRecords = records.Count
            };
        }

        /// <summary>
        /// Retrieves a student's attendance statistics.
        /// </summary>
        public async Task<AttendanceStats> GetStudentStatsAsync(int studentId, CancellationToken cancellationToken = default)
        {
            var records = await DbSet
                .Include(ar => ar.CourseSession)
                .Where(ar => ar.StudentId == studentId)
                .ToListAsync(cancellationToken);

            var totalCourseSessions = await Context.Set<CourseSession>()
                .Include(c => c.StudyGroup)
                .Where(c => c.StudyGroup.Students.Any(s => s.StudentId == studentId))
                .CountAsync(cancellationToken);

            var attendedCourseSessions = records.Count(r => r.IsConfirmed);
            var attendanceRate = totalCourseSessions > 0 ? (decimal)attendedCourseSessions / totalCourseSessions * 100 : 0;

            return new AttendanceStats
            {
                TotalCourseSessions = totalCourseSessions,
                AttendedCourseSessions = attendedCourseSessions,
                AttendanceRate = attendanceRate
            };
        }

        /// <summary>
        /// Retrieves detailed attendance records for a course session.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetDetailedByCourseSessionIdAsync(
            int courseSessionId,
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(ar => ar.Student)
                    .ThenInclude(s => s.User)
                .Include(ar => ar.CourseSession)
                    .ThenInclude(cs => cs.StudyGroup)
                .Include(ar => ar.CourseSession)
                    .ThenInclude(cs => cs.Classroom)
                .Where(ar => ar.CourseSessionId == courseSessionId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves a student's detailed attendance history.
        /// </summary>
        public async Task<IEnumerable<AttendanceRecord>> GetDetailedStudentAttendanceAsync(
            int studentId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(a => a.Student)
                    .ThenInclude(s => s.User)
                .Include(a => a.CourseSession)
                    .ThenInclude(cs => cs.StudyGroup)
                .Include(a => a.CourseSession)
                    .ThenInclude(cs => cs.Classroom)
                .Where(a => a.StudentId == studentId);

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(r =>
                    r.CheckInTime >= startDate.Value &&
                    r.CheckInTime <= endDate.Value);
            }

            return await query
                .OrderByDescending(a => a.CheckInTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves department-level attendance statistics.
        /// </summary>
        public async Task<AttendanceStats> GetDepartmentAttendanceStatsAsync(
            int departmentId,
            int? academicYearId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(ar => ar.CourseSession)
                    .ThenInclude(cs => cs.StudyGroup)
                        .ThenInclude(sg => sg.Subject)
                .Where(ar => ar.CourseSession.StudyGroup.Subject.DepartmentId == departmentId);

            if (academicYearId.HasValue)
            {
                query = query.Where(ar => ar.CourseSession.StudyGroup.AcademicYearId == academicYearId.Value);
            }

            var records = await query.ToListAsync(cancellationToken);

            return new AttendanceStats
            {
                TotalSessions = records.Select(r => r.CourseSessionId).Distinct().Count(),
                TotalStudents = records.Select(r => r.StudentId).Distinct().Count(),
                PresentCount = records.Count(r => r.IsConfirmed && !r.IsAbsent),
                AbsentCount = records.Count(r => r.IsConfirmed && r.IsAbsent),
                PendingCount = records.Count(r => !r.IsConfirmed),
                AverageAttendance = records.Any()
                    ? (decimal)records.Count(r => r.IsConfirmed && !r.IsAbsent) / records.Count * 100
                    : 0
            };
        }
    }
}