using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        Task<Classroom?> GetByReaderDeviceIdAsync(
            string deviceId, 
            CancellationToken cancellationToken = default);
    }
}