using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Identity;

/// <summary>
/// Represents a professor entity in the university system.
/// </summary>
/// <remarks>
/// This class extends the base Entity class and maintains relationships with Department and User entities.
/// </remarks>
namespace UniAttend.Core.Entities
{
    public class Professor : Entity
    {
        public Professor(int departmentId, User user)
        {
            DepartmentId = departmentId;
            UserId = user.Id;
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public int DepartmentId { get; }
        public int UserId { get; }
        public User User { get; }
        public Department? Department { get; set; } 
    }
}