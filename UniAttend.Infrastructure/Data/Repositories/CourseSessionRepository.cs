using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class CourseSessionRepository : BaseRepository<CourseSession>, ICourseSessionRepository
    {
        public CourseSessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<CourseSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(cs => cs.StudyGroup)
                .Include(cs => cs.Classroom)
                .Include(cs => cs.AttendanceRecords)
                    .ThenInclude(ar => ar.Student)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);

        public async Task<IEnumerable<CourseSession>> GetActiveSessionsAsync(
            int? courseSessionId = null,
            int? studyGroupId = null,
            int? classroomId = null,
            DateTime? date = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(cs => cs.StudyGroup)
                .Include(cs => cs.Classroom)
                .Where(cs => cs.Status == "Active");

            if (courseSessionId.HasValue)
                query = query.Where(cs => cs.Id == courseSessionId);

            if (studyGroupId.HasValue)
                query = query.Where(cs => cs.StudyGroupId == studyGroupId);

            if (classroomId.HasValue)
                query = query.Where(cs => cs.ClassroomId == classroomId);

            if (date.HasValue)
                query = query.Where(cs => cs.Date.Date == date.Value.Date);

            if (date.HasValue)
            {
                var startOfDay = date.Value.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
                query = query.Where(cs => cs.Date >= startOfDay && cs.Date <= endOfDay);
            }

            return await query
                .OrderBy(cs => cs.StartTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<CourseSession>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            int? studyGroupId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(cs => cs.StudyGroup)
                .Include(cs => cs.Classroom)
                .Where(cs => cs.Date >= startDate.Date && cs.Date <= endDate.Date);

            if (studyGroupId.HasValue)
                query = query.Where(cs => cs.StudyGroupId == studyGroupId);

            return await query
                .OrderBy(cs => cs.Date)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync(cancellationToken);
        }

        public override async Task<CourseSession> AddAsync(CourseSession entity, CancellationToken cancellationToken = default)
        {
            await ValidateSessionAsync(entity, cancellationToken);
            return await base.AddAsync(entity, cancellationToken);
        }

        public override async Task UpdateAsync(CourseSession entity, CancellationToken cancellationToken = default)
        {
            await ValidateSessionAsync(entity, cancellationToken);
            await base.UpdateAsync(entity, cancellationToken);
        }

        private async Task ValidateSessionAsync(CourseSession session, CancellationToken cancellationToken)
        {
            var conflictingSession = await DbSet
                .AnyAsync(cs =>
                    cs.Id != session.Id &&
                    cs.ClassroomId == session.ClassroomId &&
                    cs.Date == session.Date &&
                    ((cs.StartTime <= session.StartTime && cs.EndTime > session.StartTime) ||
                     (cs.StartTime < session.EndTime && cs.EndTime >= session.EndTime)),
                    cancellationToken);

            if (conflictingSession)
                throw new InvalidOperationException("There is a conflicting session in this classroom at the specified time.");
        }

        public async Task<CourseSession?> GetActiveByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(cs => cs.StudyGroup)
                .Include(cs => cs.Classroom)
                .FirstOrDefaultAsync(cs =>
                    cs.Classroom.ReaderDeviceId == deviceId &&
                    cs.IsActive &&
                    cs.Date.Date == DateTime.Today,
                    cancellationToken);
        }
    }
}