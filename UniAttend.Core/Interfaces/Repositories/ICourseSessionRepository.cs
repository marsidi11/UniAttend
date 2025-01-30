using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface ICourseSessionRepository : IRepository<CourseSession>
    {
        Task<CourseSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CourseSession?> GetActiveByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);

        Task<IEnumerable<CourseSession>> GetActiveSessionsAsync(
            int? courseSessionId = null,
            int? studyGroupId = null,
            int? classroomId = null, 
            DateTime? date = null,
            CancellationToken cancellationToken = default);
            
        Task<IEnumerable<CourseSession>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            int? studyGroupId = null,
            CancellationToken cancellationToken = default);
    }
}