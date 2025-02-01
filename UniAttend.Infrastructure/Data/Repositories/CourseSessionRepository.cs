using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing CourseSession entities.
    /// </summary>
    public class CourseSessionRepository : BaseRepository<CourseSession>, ICourseSessionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CourseSessionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CourseSessionRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Retrieves a CourseSession by its unique identifier.
        /// </summary>
        /// <param name="id">The session identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The matching CourseSession entity or null if not found.</returns>
        public override async Task<CourseSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(cs => cs.StudyGroup)
                .Include(cs => cs.Classroom)
                .Include(cs => cs.AttendanceRecords)
                    .ThenInclude(ar => ar.Student)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);

        /// <summary>
        /// Retrieves active CourseSessions based on provided filters.
        /// </summary>
        /// <param name="courseSessionId">Optional session identifier filter.</param>
        /// <param name="studyGroupId">Optional study group identifier filter.</param>
        /// <param name="classroomId">Optional classroom identifier filter.</param>
        /// <param name="professorId">Optional professor identifier filter.</param>
        /// <param name="date">Optional date filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of active CourseSession entities.</returns>
        public async Task<IEnumerable<CourseSession>> GetActiveSessionsAsync(
            int? courseSessionId = null,
            int? studyGroupId = null,
            int? classroomId = null,
            int? professorId = null,
            DateTime? date = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(cs => cs.StudyGroup)
                    .ThenInclude(sg => sg.Professor)
                .Include(cs => cs.Classroom)
                .Where(cs => cs.Status == SessionStatus.Active);

            if (courseSessionId.HasValue)
                query = query.Where(cs => cs.Id == courseSessionId);

            if (studyGroupId.HasValue)
                query = query.Where(cs => cs.StudyGroupId == studyGroupId);

            if (classroomId.HasValue)
                query = query.Where(cs => cs.ClassroomId == classroomId);

            if (professorId.HasValue)
                query = query.Where(cs => cs.StudyGroup.ProfessorId == professorId.Value);

            if (date.HasValue)
            {
                var startOfDay = date.Value.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
                query = query.Where(cs => cs.Date >= startOfDay && cs.Date < endOfDay);
            }

            var result = await query
                .OrderBy(cs => cs.StartTime)
                .ToListAsync(cancellationToken);

            return result;
        }

        /// <summary>
        /// Retrieves CourseSessions within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date for filtering.</param>
        /// <param name="endDate">The end date for filtering.</param>
        /// <param name="studyGroupId">Optional study group identifier filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of CourseSession entities matching the criteria.</returns>
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

        /// <summary>
        /// Adds a new CourseSession after validating it.
        /// </summary>
        /// <param name="entity">The CourseSession entity to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created CourseSession entity.</returns>
        public override async Task<CourseSession> AddAsync(CourseSession entity, CancellationToken cancellationToken = default)
        {
            await ValidateSessionAsync(entity, cancellationToken);
            return await base.AddAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Updates an existing CourseSession after validating it.
        /// </summary>
        /// <param name="entity">The CourseSession entity to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public override async Task UpdateAsync(CourseSession entity, CancellationToken cancellationToken = default)
        {
            await ValidateSessionAsync(entity, cancellationToken);
            await base.UpdateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Validates the CourseSession to ensure there are no time conflicts.
        /// </summary>
        /// <param name="session">The CourseSession entity to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="InvalidOperationException">Thrown when a conflicting session is detected.</exception>
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

        /// <summary>
        /// Retrieves an active CourseSession by device identifier.
        /// </summary>
        /// <param name="deviceId">The device identifier associated with the classroom reader.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The matching active CourseSession entity or null if not found.</returns>
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

        /// <summary>
        /// Retrieves a CourseSession by its unique identifier with related Classroom entity.
        /// </summary>
        public async Task<CourseSession> GetByIdWithClassroomAsync(int courseSessionId)
        {
            var session = await DbSet
                .Include(x => x.Classroom)
                .FirstOrDefaultAsync(x => x.Id == courseSessionId);
        
            if (session == null)
            {
                throw new KeyNotFoundException($"Course session with ID {courseSessionId} not found");
            }
        
            return session;
        }
    }
}