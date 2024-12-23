using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents a subject within a department, including its name, associated department, and study groups.
    /// </summary>
    public class Subject : ActiveEntity
    {
        private Subject() 
        {
            Name = string.Empty;
            Department = null!;
            StudyGroups = new List<StudyGroup>();
        }

        public Subject(string name, Department department)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (name.Length > 255)
                throw new ArgumentException("Name cannot exceed 255 characters", nameof(name));

            Name = name;
            Department = department ?? throw new ArgumentNullException(nameof(department));
            DepartmentId = department.Id;
            StudyGroups = new List<StudyGroup>();
        }

        public string Name { get; private init; }
        public int DepartmentId { get; private init; }
        public Department Department { get; private init; }
        public ICollection<StudyGroup> StudyGroups { get; private init; }
    }
}