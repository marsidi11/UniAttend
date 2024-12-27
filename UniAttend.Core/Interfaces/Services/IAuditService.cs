using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniAttend.Core.Entities.Audit;

/// <summary>
/// Service responsible for managing and retrieving audit logs in the system.
/// </summary>
namespace UniAttend.Core.Interfaces.Services
{
    public interface IAuditService
    {
        Task LogActionAsync(string action, string entityType, int entityId, 
            int userId, string details, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuditLog>> GetUserActionsAsync(int userId, 
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}