/// <summary>
/// Base entity for objects that can be marked as active/inactive in the system.
/// Provides soft-delete functionality through IsActive flag.
/// </summary>
namespace UniAttend.Core.Entities.Base
{
    public abstract class ActiveEntity : Entity
    {
        public bool IsActive { get; protected set; } = true;

        protected void Deactivate() => IsActive = false;
        protected void Activate() => IsActive = true;
    }
}