using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents an academic department in the university
    /// </summary>
    public class Department : ActiveEntity
    {
        private Department() { } // EF Core

        public Department(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            Name = name;
        }

        public string Name { get; private set; }
        
        public ICollection<Subject> Subjects { get; private set; } = new List<Subject>();
        public ICollection<Student> Students { get; private set; } = new List<Student>();
        public ICollection<Professor> Professors { get; private set; } = new List<Professor>();
    }
}