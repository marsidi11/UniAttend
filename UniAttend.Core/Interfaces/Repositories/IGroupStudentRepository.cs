using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IGroupStudentRepository : IRepository<GroupStudent>
    {
        Task<IEnumerable<GroupStudent>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GroupStudent>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int groupId, int studentId, CancellationToken cancellationToken = default);
        Task DeleteAsync(int groupId, int studentId, CancellationToken cancellationToken = default);
    }
}