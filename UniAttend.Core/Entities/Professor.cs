using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Professor : Entity
    {
        private Professor() { } // For domain events/serialization
    
        public Professor(int departmentId, User user)
        {
            ValidateUser(user);
            
            Id = user.Id;
            DepartmentId = departmentId;
            User = user;

            CreatedAt = DateTime.UtcNow; // Set creation time
        }
    
        // Identity properties - immutable
        public int DepartmentId { get; }
        
        // Domain references without EF Core annotations - nullable for EF Core
        public User? User { get; private set; }
        public Department? Department { get; private set; }
    
        // Domain validation
        private static void ValidateUser(User user)
        {
            if (user == null)
                throw new DomainException("User cannot be null");
        }
    
        // Domain methods
        public void AssignDepartment(Department department)
        {
            Department = department ?? throw new DomainException("Department cannot be null");
        }
    }
}