using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents a subject within a department, including its name, description, credits and associated department.
    /// </summary>
    public class Subject : ActiveEntity
    {
        private readonly List<StudyGroup> _studyGroups = new();

        private Subject()
        {
            Name = string.Empty;
            Description = string.Empty;
            Department = null!;
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
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Credits { get; private set; }
        public int DepartmentId { get; private init; }
        public Department Department { get; private init; }
        public IReadOnlyCollection<StudyGroup> StudyGroups => _studyGroups.AsReadOnly();

        public void Update(string name, string description, int credits)
        {
            ValidateName(name);
            ValidateCredits(credits);

            Name = name;
            Description = description ?? string.Empty;
            Credits = credits;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Subject name cannot be empty");
            if (name.Length > 255)
                throw new DomainException("Subject name cannot exceed 255 characters");
        }

        private static void ValidateCredits(int credits)
        {
            if (credits < 0)
                throw new DomainException("Credits cannot be negative");
            if (credits > 30)
                throw new DomainException("Credits cannot exceed 30");
        }
    }
}