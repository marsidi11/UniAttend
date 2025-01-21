using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing Schedule entities in the database.
    /// Handles academic timetabling, classroom allocation, and schedule conflict detection.
    /// </summary>
    /// <remarks>
    /// This repository is part of the Infrastructure layer and implements the DDD pattern.
    /// It manages the persistence of schedule data and provides specialized queries for
    /// academic scheduling operations.
    /// </remarks>
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Schedule?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.StudyGroup)
                .Include(s => s.Classroom)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

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

        public async Task<bool> HasGroupConflictAsync(
            int groupId,
            int dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeScheduleId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.Where(s => 
                s.StudyGroupId == groupId && 
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