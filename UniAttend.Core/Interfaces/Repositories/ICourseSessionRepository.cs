using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface ICourseSessionRepository : IRepository<CourseSession>
    {
        Task<IEnumerable<CourseSession>> GetActiveSessionsAsync(
            int? groupId = null,
            int? classroomId = null,
            DateTime? date = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<CourseSession>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            int? groupId = null,
            CancellationToken cancellationToken = default);
    }
}