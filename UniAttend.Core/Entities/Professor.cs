using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Professor : Entity
    {
        private readonly List<Department> _departments = new();

        private Professor() { } // For domain events/serialization

        public Professor(User user)
        {
            ValidateUser(user);

            Id = user.Id;
            User = user;

            CreatedAt = DateTime.UtcNow; // Set creation time
        }

        // Domain references without EF Core annotations - nullable for EF Core
        public User? User { get; private set; }
        public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();

        // Domain validation
        private static void ValidateUser(User user)
        {
            if (user == null)
                throw new DomainException("User cannot be null");
        }

        // Domain methods
        public void AddDepartment(Department department)
        {
            if (department == null)
                throw new DomainException("Department cannot be null");

            if (!_departments.Contains(department))
                _departments.Add(department);
        }

        public void RemoveDepartment(Department department)
        {
            _departments.Remove(department);
        }
    }
}