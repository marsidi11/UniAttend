/// <summary>
/// Base entity for objects that can be marked as active/inactive in the system.
/// Provides soft-delete functionality through IsActive flag.
/// </summary>
namespace UniAttend.Core.Entities.Base
{
    public abstract class ActiveEntity : Entity
    {
        protected ActiveEntity()
        {
            IsActive = true;
        }

        protected ActiveEntity(bool isActive)
        {
            IsActive = isActive;
        }

        public bool IsActive { get; protected set; }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetDeactive()
        {
            IsActive = false;
        }

        protected void Deactivate() => IsActive = false;
        protected void Activate() => IsActive = true;
    }
}