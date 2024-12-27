using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Audit;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<AuditLog>().FindAsync(new object[] { id }, cancellationToken);

        public async Task<IEnumerable<AuditLog>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Set<AuditLog>().ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<AuditLog>().AnyAsync(x => x.Id == id, cancellationToken);

        public async Task<AuditLog> AddAsync(AuditLog entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<AuditLog>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(AuditLog entity, CancellationToken cancellationToken = default)
        {
            _context.Set<AuditLog>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var audit = await GetByIdAsync(id, cancellationToken);
            if (audit != null)
            {
                _context.Set<AuditLog>().Remove(audit);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAndDateRangeAsync(
            int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<AuditLog>()
                .Where(a => a.UserId == userId && 
                           a.Timestamp >= startDate && 
                           a.Timestamp <= endDate)
                .ToListAsync(cancellationToken);
        }
    }
}