using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IProfessorRepository : IRepository<Professor>
    {
        Task<Professor?> GetByUserId(int userId, CancellationToken cancellationToken = default);
        Task<Professor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Professor>> GetByDepartmentId(int departmentId, CancellationToken cancellationToken = default);
        Task<Professor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}