using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Classroom?> GetByReaderDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Classroom>> GetAvailableAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);
    }
}