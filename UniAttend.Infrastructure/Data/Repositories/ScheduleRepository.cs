using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing Schedule entities.
    /// </summary>
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        /// <summary>
        /// Initializes a new instance of the ScheduleRepository class.
        /// </summary>
        public ScheduleRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves all schedules with related details.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Subject)
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Professor)
                        .ThenInclude(p => p.User)
                .Include(s => s.Classroom)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a schedule by its identifier with related details.
        /// </summary>
        public override async Task<Schedule?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.StudyGroup)
                .Include(s => s.Classroom)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        /// <summary>
        /// Retrieves schedules for a specific professor.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetByProfessorIdAsync(
            int professorId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Subject)
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Professor)
                        .ThenInclude(p => p.User)
                .Include(s => s.Classroom)
                .Where(s => s.StudyGroup.ProfessorId == professorId)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves schedules for a specific study group.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(s => s.StudyGroupId == studyGroupId)
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Subject)
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Professor)
                        .ThenInclude(p => p.User)
                .Include(s => s.Classroom)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves schedules for a specific classroom.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetByClassroomIdAsync(int classroomId, CancellationToken cancellationToken)
        {
            return await DbSet
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Subject)
                .Include(s => s.StudyGroup)
                    .ThenInclude(g => g.Professor)
                        .ThenInclude(p => p.User)
                .Include(s => s.Classroom)
                .Where(s => s.ClassroomId == classroomId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if there is any time conflict for a schedule in a classroom.
        /// </summary>
        public async Task<bool> HasTimeConflictAsync(
            int classroomId,
            int dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeScheduleId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.Where(s =>
                s.ClassroomId == classroomId &&
                s.DayOfWeek == dayOfWeek);

            if (excludeScheduleId.HasValue)
            {
                query = query.Where(s => s.Id != excludeScheduleId.Value);
            }

            return await query.AnyAsync(s =>
                (startTime >= s.StartTime && startTime < s.EndTime) ||
                (endTime > s.StartTime && endTime <= s.EndTime) ||
                (startTime <= s.StartTime && endTime >= s.EndTime),
                cancellationToken);
        }

        /// <summary>
        /// Checks if there is any classroom scheduling conflict.
        /// </summary>
        public async Task<bool> HasClassroomConflictAsync(
            int classroomId,
            int dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeScheduleId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.Where(s =>
                s.ClassroomId == classroomId &&
                s.DayOfWeek == dayOfWeek);

            if (excludeScheduleId.HasValue)
            {
                query = query.Where(s => s.Id != excludeScheduleId.Value);
            }

            return await query.AnyAsync(s =>
                (startTime >= s.StartTime && startTime < s.EndTime) ||
                (endTime > s.StartTime && endTime <= s.EndTime) ||
                (startTime <= s.StartTime && endTime >= s.EndTime),
                cancellationToken);
        }

        /// <summary>
        /// Checks if there is any scheduling conflict for a study group.
        /// </summary>
        public async Task<bool> HasGroupConflictAsync(
            int studyGroupId,
            int dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeScheduleId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.Where(s =>
                s.StudyGroupId == studyGroupId &&
                s.DayOfWeek == dayOfWeek);

            if (excludeScheduleId.HasValue)
            {
                query = query.Where(s => s.Id != excludeScheduleId.Value);
            }

            return await query.AnyAsync(s =>
                (startTime >= s.StartTime && startTime < s.EndTime) ||
                (endTime > s.StartTime && endTime <= s.EndTime) ||
                (startTime <= s.StartTime && endTime >= s.EndTime),
                cancellationToken);
        }
    }
}