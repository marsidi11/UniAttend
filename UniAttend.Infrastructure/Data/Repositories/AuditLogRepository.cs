using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Audit;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext context) : base(context) { }
        
        public async Task<IEnumerable<AuditLog>> GetByUserAndDateRangeAsync(
            int userId, 
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(a => a.UserId == userId && 
                           a.Timestamp >= startDate && 
                           a.Timestamp <= endDate)
                .ToListAsync(cancellationToken);
        }
    }
}