using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    public class StudyGroup : ActiveEntity
    {
        public StudyGroup(string name, int subjectId, int academicYearId, int professorId, bool isActive = true) 
            : base(isActive)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SubjectId = subjectId;
            AcademicYearId = academicYearId;
            ProfessorId = professorId;
        }

        public string Name { get; }
        public int SubjectId { get; }
        public int AcademicYearId { get; }
        public int ProfessorId { get; }

        // Navigation properties
        public Subject Subject { get; private set; } = null!;
        public AcademicYear AcademicYear { get; private set; } = null!;
        public Professor Professor { get; private set; } = null!;
        public ICollection<GroupStudent> Students { get; private init; } = new List<GroupStudent>();
        public ICollection<AbsenceAlert> AbsenceAlerts { get; private init; } = new List<AbsenceAlert>();
    }
}