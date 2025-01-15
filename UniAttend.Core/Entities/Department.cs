using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents an academic department in the university, managing subjects, students and professors.
    /// </summary>
    /// <remarks>
    /// A department is a fundamental organizational unit that groups related subjects
    /// and academic staff. It maintains relationships with subjects taught, enrolled students,
    /// and affiliated professors.
    /// </remarks>
    public class Department : ActiveEntity 
    {
        private readonly List<Subject> _subjects = new();
        private readonly List<Student> _students = new();
        private readonly List<Professor> _professors = new();

        // Constructor for domain logic
        public Department(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Department name cannot be empty", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Department name cannot exceed 100 characters", nameof(name));

            Name = name;
            CreatedAt = DateTime.UtcNow; // Set creation time
        }

        /// <summary>
        /// Gets the name of the department.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the collection of subjects offered by this department.
        /// </summary>
        public IReadOnlyCollection<Subject> Subjects => _subjects.AsReadOnly();

        /// <summary>
        /// Gets the collection of students enrolled in this department.
        /// </summary>
        public IReadOnlyCollection<Student> Students => _students.AsReadOnly();

        /// <summary>
        /// Gets the collection of professors affiliated with this department.
        /// </summary>
        public IReadOnlyCollection<Professor> Professors => _professors.AsReadOnly();

        /// <summary>
        /// Updates the department's name while ensuring business rules.
        /// </summary>
        /// <param name="newName">The new name for the department.</param>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Department name cannot be empty", nameof(newName));
            if (newName.Length > 100)
                throw new ArgumentException("Department name cannot exceed 100 characters", nameof(newName));

            Name = newName;
        }

        /// <summary>
        /// Adds a subject to the department.
        /// </summary>
        public void AddSubject(Subject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            
            _subjects.Add(subject);
        }

        /// <summary>
        /// Enrolls a student in the department.
        /// </summary>
        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            
            _students.Add(student);
        }

        /// <summary>
        /// Assigns a professor to the department.
        /// </summary>
        public void AddProfessor(Professor professor)
        {
            if (professor == null)
                throw new ArgumentNullException(nameof(professor));

            _professors.Add(professor);
        }

        /// <summary>
        /// Marks the department as inactive (soft-delete).
        /// </summary>
        public new void Deactivate() => base.Deactivate();
    }
}