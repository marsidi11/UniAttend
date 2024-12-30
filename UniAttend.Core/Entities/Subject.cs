using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents a subject within a department, including its name, description, credits and associated department.
    /// </summary>
    public class Subject : ActiveEntity
    {
        private Subject() 
        {
            Name = string.Empty;
            Description = string.Empty;
            Department = null!;
            StudyGroups = new List<StudyGroup>();
        }

        public Subject(string name, string description, int credits, Department department)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (name.Length > 255)
                throw new ArgumentException("Name cannot exceed 255 characters", nameof(name));
            if (credits < 0)
                throw new ArgumentException("Credits cannot be negative", nameof(credits));
            if (credits > 30)
                throw new ArgumentException("Credits cannot exceed 30", nameof(credits));

            Name = name;
            Description = description ?? string.Empty;
            Credits = credits;
            Department = department ?? throw new ArgumentNullException(nameof(department));
            DepartmentId = department.Id;
            StudyGroups = new List<StudyGroup>();
        }

        public string Name { get; private init; }
        public string Description { get; private init; }
        public int Credits { get; private init; }
        public int DepartmentId { get; private init; }
        public Department Department { get; private init; }
        public ICollection<StudyGroup> StudyGroups { get; private init; }
    }
}