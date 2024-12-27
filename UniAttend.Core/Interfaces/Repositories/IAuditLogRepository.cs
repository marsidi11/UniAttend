using UniAttend.Core.Entities.Audit;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByUserAndDateRangeAsync(
            int userId, 
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
    }
}