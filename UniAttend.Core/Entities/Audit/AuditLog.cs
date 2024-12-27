using System;
using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities.Audit
{
    public class AuditLog : Entity
    {
        public string Action { get; private set; }
        public string EntityType { get; private set; }
        public int EntityId { get; private set; }
        public int UserId { get; private set; }
        public string Details { get; private set; }
        public DateTime Timestamp { get; private set; }

        public AuditLog(string action, string entityType, int entityId, int userId, string details, DateTime timestamp)
        {
            Action = action;
            EntityType = entityType;
            EntityId = entityId;
            UserId = userId;
            Details = details;
            Timestamp = timestamp;
        }
    }
}