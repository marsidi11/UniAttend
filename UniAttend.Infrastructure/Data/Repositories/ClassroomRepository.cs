using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class ClassroomRepository : BaseRepository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        public async Task<Classroom?> GetByReaderDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(c => c.ReaderDeviceId == deviceId, cancellationToken);
        }

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