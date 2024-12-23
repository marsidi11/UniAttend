using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    public class StudyGroup : Entity
    {
        public StudyGroup(string name, int subjectId, int academicYearId, int professorId)
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

        public Subject Subject { get; set; } // Navigation property
        public AcademicYear AcademicYear { get; set; } // Navigation property
        public Professor Professor { get; set; } // Navigation property

    }
}