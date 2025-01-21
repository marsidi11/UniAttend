using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Schedule>> GetByClassroomIdAsync(int classroomId, CancellationToken cancellationToken = default);

        Task<bool> HasTimeConflictAsync(
            int classroomId,
            int dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeScheduleId = default,
            CancellationToken cancellationToken = default);

        Task<bool> HasClassroomConflictAsync(int classroomId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeScheduleId = null, CancellationToken cancellationToken = default);

        Task<bool> HasGroupConflictAsync(int groupId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeScheduleId = null, CancellationToken cancellationToken = default);
    }


}