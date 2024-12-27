using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniAttend.Core.Entities.Audit;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditRepository;
        private readonly ICurrentUserService _currentUserService;

        public AuditService(IAuditLogRepository auditRepository, ICurrentUserService currentUserService)
        {
            _auditRepository = auditRepository;
            _currentUserService = currentUserService;
        }

        public async Task LogActionAsync(string action, string entityType, int entityId, 
            int userId, string details, CancellationToken cancellationToken = default)
        {
            var log = new AuditLog(action, entityType, entityId, userId, details, DateTime.UtcNow);
            await _auditRepository.AddAsync(log, cancellationToken);
        }

        public async Task<IEnumerable<AuditLog>> GetUserActionsAsync(int userId, 
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _auditRepository.GetByUserAndDateRangeAsync(userId, startDate, endDate, cancellationToken);
        }
    }
}