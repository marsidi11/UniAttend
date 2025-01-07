using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Attendance;

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
        public virtual Subject Subject { get; private set; } = null!;
        public virtual AcademicYear AcademicYear { get; private set; } = null!;
        public virtual Professor Professor { get; private set; } = null!;
        public virtual ICollection<GroupStudent> Students { get; private init; } = new List<GroupStudent>();
        public virtual ICollection<Schedule> Schedules { get; private init; } = new List<Schedule>();
        public virtual ICollection<AttendanceRecord> AttendanceRecords { get; private init; } = new List<AttendanceRecord>();
    }
}