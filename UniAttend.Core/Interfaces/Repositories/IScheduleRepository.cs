using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Schedule>> GetByClassroomIdAsync(int classroomId, CancellationToken cancellationToken = default);
        Task<bool> HasTimeConflictAsync(int classroomId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeScheduleId = null, CancellationToken cancellationToken = default);
    }
}