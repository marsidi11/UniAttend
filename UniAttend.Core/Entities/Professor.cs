using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Identity;

namespace UniAttend.Core.Entities
{
    public class Professor : Entity
    {
        public Professor(int departmentId, User user)
        {
            DepartmentId = departmentId;
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
        public int DepartmentId { get; }
        public User User { get; } // Navigation property
        public Department Department { get; set; } // Navigation property

    }
}