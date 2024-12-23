using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Identity;

namespace UniAttend.Core.Entities
{
    public class Student : Entity
    {
        public Student(string studentId, int departmentId, User user)
        {
            StudentId = studentId ?? throw new ArgumentNullException(nameof(studentId));
            DepartmentId = departmentId;
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public string StudentId { get; }
        public string? CardId { get; set; }
        public int DepartmentId { get; }

        public User User { get; } // Navigation property
        public Department Department { get; set; } // Navigation property
    }
}