using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
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

        public async Task<IEnumerable<AttendanceRecord>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.CourseId == courseId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.StudentId == studentId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.CheckInTime >= startDate && a.CheckInTime <= endDate)
                .ToListAsync(cancellationToken);

        public async Task<AttendanceRecord?> GetStudentAttendanceForCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
            => await DbSet
                .FirstOrDefaultAsync(a => a.StudentId == studentId && a.CourseId == courseId, cancellationToken);

        public async Task<double> GetStudentAttendancePercentageAsync(int studentId, int groupId, CancellationToken cancellationToken = default)
        {
            var totalClasses = await Context.Set<Course>()
                .CountAsync(c => c.StudyGroupId == groupId && c.IsActive, cancellationToken);

            if (totalClasses == 0) return 0;

            var attendedClasses = await DbSet
                .CountAsync(a => a.StudentId == studentId && a.IsConfirmed, cancellationToken);

            return (double)attendedClasses / totalClasses * 100;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(int courseId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(a => a.CourseId == courseId && !a.IsConfirmed)
                .ToListAsync(cancellationToken);

        public async Task ConfirmAttendanceRecordsAsync(int courseId, CancellationToken cancellationToken = default)
        {
            var records = await GetUnconfirmedRecordsAsync(courseId, cancellationToken);
            foreach (var record in records)
            {
                record.Confirm();
                DbSet.Update(record);
            }
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<AttendanceRecord>> GetGroupAttendanceAsync(
        int groupId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(ar => ar.Student)
                    .ThenInclude(s => s.User)
                .Include(ar => ar.Course)
                    .ThenInclude(c => c.StudyGroup)
                .Where(ar =>
                    ar.Course.StudyGroupId == groupId &&
                    ar.CheckInTime >= startDate &&
                    ar.CheckInTime <= endDate)
                .OrderByDescending(ar => ar.CheckInTime)
                .ThenBy(ar => ar.Student.User.LastName)
                .ThenBy(ar => ar.Student.User.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<CourseSession> GetSessionWithDetailsAsync(
    int sessionId,
    CancellationToken cancellationToken = default)
        {
            var session = await Context.Set<CourseSession>()
                .Include(cs => cs.Group)
                    .ThenInclude(g => g.Students)
                        .ThenInclude(gs => gs.Student) // Add this level
                            .ThenInclude(s => s.User) // Then access User through Student
                .FirstOrDefaultAsync(cs => cs.Id == sessionId, cancellationToken);

            if (session == null)
            {
                throw new KeyNotFoundException($"Course session with ID {sessionId} not found");
            }

            return session;
        }
    }
}