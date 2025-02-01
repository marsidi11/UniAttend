using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing Classroom entities.
    /// </summary>
    public class ClassroomRepository : BaseRepository<Classroom>, IClassroomRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ClassroomRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Checks if a classroom exists with the specified name.
        /// </summary>
        /// <param name="name">Name of the classroom.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if a classroom with the given name exists; otherwise, false.</returns>
        public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        /// <summary>
        /// Retrieves a classroom by its reader device identifier.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A matching <see cref="Classroom"/> or null if not found.</returns>
        public async Task<Classroom?> GetByReaderDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(c => c.ReaderDeviceId == deviceId, cancellationToken);
        }

        /// <summary>
        /// Gets available classrooms within the specified time interval.
        /// </summary>
        /// <param name="startTime">Start time of the interval.</param>
        /// <param name="endTime">End time of the interval.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of available <see cref="Classroom"/> entities.</returns>
        public async Task<IEnumerable<Classroom>> GetAvailableAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
        {
            var date = startTime.Date;
            var startTimeOfDay = startTime.TimeOfDay;
            var endTimeOfDay = endTime.TimeOfDay;

            var busyClassroomIds = await Context.Set<Schedule>()
                .Where(s => s.DayOfWeek == (int)startTime.DayOfWeek &&
                    ((s.StartTime <= startTimeOfDay && s.EndTime > startTimeOfDay) ||
                     (s.StartTime < endTimeOfDay && s.EndTime >= endTimeOfDay) ||
                     (s.StartTime >= startTimeOfDay && s.EndTime <= endTimeOfDay)))
                .Select(s => s.ClassroomId)
                .Distinct()
                .ToListAsync(cancellationToken);

            return await DbSet
                .Where(c => !busyClassroomIds.Contains(c.Id))
                .ToListAsync(cancellationToken);
        }
    }
}