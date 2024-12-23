/// <summary>
/// Base entity providing fundamental identity for all entities in the system.
/// </summary>
namespace UniAttend.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
    }
}