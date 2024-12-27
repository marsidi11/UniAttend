using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

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
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public ScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Schedule?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<Schedule>()
                .Include(s => s.Group)
                .Include(s => s.Classroom)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public async Task<IEnumerable<Schedule>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Set<Schedule>()
                .Include(s => s.Group)
                .Include(s => s.Classroom)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<Schedule>()
                .AnyAsync(s => s.Id == id, cancellationToken);

        public async Task<Schedule> AddAsync(Schedule entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<Schedule>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Schedule entity, CancellationToken cancellationToken = default)
        {
            _context.Set<Schedule>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var schedule = await GetByIdAsync(id, cancellationToken);
            if (schedule != null)
            {
                _context.Set<Schedule>().Remove(schedule);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<Schedule>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
            => await _context.Set<Schedule>()
                .Where(s => s.GroupId == groupId)
                .Include(s => s.Classroom)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Schedule>> GetByClassroomIdAsync(int classroomId, CancellationToken cancellationToken = default)
            => await _context.Set<Schedule>()
                .Where(s => s.ClassroomId == classroomId)
                .Include(s => s.Group)
                .ToListAsync(cancellationToken);

        public async Task<bool> HasTimeConflictAsync(
            int classroomId, 
            int dayOfWeek, 
            TimeSpan startTime, 
            TimeSpan endTime, 
            int? excludeScheduleId = null, 
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<Schedule>()
                .Where(s => s.ClassroomId == classroomId
                    && s.DayOfWeek == dayOfWeek
                    && ((s.StartTime <= startTime && s.EndTime > startTime)
                    || (s.StartTime < endTime && s.EndTime >= endTime)
                    || (s.StartTime >= startTime && s.EndTime <= endTime)));

            if (excludeScheduleId.HasValue)
                query = query.Where(s => s.Id != excludeScheduleId.Value);

            return await query.AnyAsync(cancellationToken);
        }
    }
}