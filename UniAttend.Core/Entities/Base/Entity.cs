/// <summary>
/// Base entity providing fundamental identity for all entities in the system.
/// </summary>
namespace UniAttend.Core.Entities.Base
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}