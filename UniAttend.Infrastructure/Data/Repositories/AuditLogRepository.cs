using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Audit;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing AuditLog entities.
    /// </summary>
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AuditLogRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves audit logs for a user within a specified date range.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="startDate">Start date for the filter.</param>
        /// <param name="endDate">End date for the filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of matching <see cref="AuditLog"/> entries.</returns>
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