using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Stats;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing attendance records in the database.
    /// Handles CRUD operations and specialized queries for attendance tracking.
    /// </summary>
    public class AttendanceRecordRepository : BaseRepository<AttendanceRecord>, IAttendanceRecordRepository
    {
        public AttendanceRecordRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<AttendanceRecord>> GetByCourseSessionIdAsync(int courseSessionId, CancellationToken cancellationToken = default)
        => await DbSet
            .Where(a => a.CourseSessionId == courseSessionId)
            .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.StudentId == studentId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.CheckInTime >= startDate && a.CheckInTime <= endDate)
                .ToListAsync(cancellationToken);

        public async Task<AttendanceRecord?> GetStudentAttendanceForSessionAsync(
        int studentId,
        int courseSessionId,
        CancellationToken cancellationToken = default)
        => await DbSet
            .FirstOrDefaultAsync(a =>
                a.StudentId == studentId &&
                a.CourseSessionId == courseSessionId,
                cancellationToken);

        public async Task<double> GetStudentAttendancePercentageAsync(
        int studentId,
        int studyGroupId,
        CancellationToken cancellationToken = default)
        {
            var totalSessions = await Context.Set<CourseSession>()
                .CountAsync(cs =>
                    cs.StudyGroupId == studyGroupId &&
                    cs.IsActive,
                    cancellationToken);

            if (totalSessions == 0) return 0;

            var attendedSessions = await DbSet
                .CountAsync(a =>
                    a.StudentId == studentId &&
                    a.IsConfirmed &&
                    a.CourseSession.StudyGroupId == studyGroupId,
                    cancellationToken);

            return (double)attendedSessions / totalSessions * 100;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(
        int courseSessionId,
        CancellationToken cancellationToken = default)
        => await DbSet
            .Where(a =>
                a.CourseSessionId == courseSessionId &&
                !a.IsConfirmed)
            .ToListAsync(cancellationToken);

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

        public async Task<IEnumerable<AttendanceRecord>> GetGroupAttendanceAsync(
        int studyGroupId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
        {
            return await DbSet
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
        }

        public async Task<AttendanceRecord?> GetStudentAttendanceForCourseSessionAsync(
        int studentId,
        int courseSessionId,
        CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(a =>
                    a.StudentId == studentId &&
                    a.CourseSessionId == courseSessionId,
                    cancellationToken);
        }

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

            var attendedCourseSessions = attendance.Count(a => a.IsConfirmed);

            return (TotalCourseSessions: totalCourseSessions, AttendedCourseSessions: attendedCourseSessions);
        }

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
            var attendanceRate = totalCourseSessions > 0
                ? (decimal)attendedCourseSessions / totalCourseSessions * 100
                : 0;

            return new AttendanceStats
            {
                TotalCourseSessions = totalCourseSessions,
                AttendedCourseSessions = attendedCourseSessions,
                AttendanceRate = attendanceRate
            };
        }

        public async Task<IEnumerable<AttendanceRecord>> GetDetailedByCourseSessionIdAsync(
        int courseSessionId,
        CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(ar => ar.Student)
                    .ThenInclude(s => s.User)
                .Include(ar => ar.CourseSession)
                    .ThenInclude(cs => cs.StudyGroup)
                .Include(ar => ar.CourseSession)
                    .ThenInclude(cs => cs.Classroom)
                .Where(ar => ar.CourseSessionId == courseSessionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AttendanceRecord>> GetDetailedStudentAttendanceAsync(
        int studentId,
        DateTime? startDate,
        DateTime? endDate,
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
    }
}