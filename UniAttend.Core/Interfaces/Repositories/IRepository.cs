using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        // Read operations
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        // Write operations 
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        IQueryable<T> GetQueryable();
    }
}