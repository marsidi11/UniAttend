using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Schedule>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Schedule>> GetByProfessorIdAsync(
        int professorId, 
        CancellationToken cancellationToken = default);

        Task<IEnumerable<Schedule>> GetByClassroomIdAsync(int classroomId, CancellationToken cancellationToken = default);

        Task<bool> HasTimeConflictAsync(
            int classroomId,
            int dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeScheduleId = default,
            CancellationToken cancellationToken = default);

        Task<bool> HasClassroomConflictAsync(int classroomId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeScheduleId = null, CancellationToken cancellationToken = default);

        Task<bool> HasGroupConflictAsync(int studyGroupId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeScheduleId = null, CancellationToken cancellationToken = default);
    }


}