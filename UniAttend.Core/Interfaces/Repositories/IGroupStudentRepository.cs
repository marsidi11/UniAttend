using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IGroupStudentRepository : IRepository<GroupStudent>
    {
        Task<IEnumerable<GroupStudent>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GroupStudent>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int groupId, int studentId, CancellationToken cancellationToken = default);
        Task AddStudentToGroupAsync(int groupId, int studentId, CancellationToken cancellationToken = default);
        Task RemoveStudentFromGroupAsync(int groupId, int studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<GroupStudent>> GetActiveGroupsByStudentIdAsync(int studentId, int academicYearId, CancellationToken cancellationToken = default);
    }
}