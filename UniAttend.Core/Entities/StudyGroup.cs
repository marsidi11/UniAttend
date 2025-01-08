using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class StudyGroup : ActiveEntity
    {
        private readonly List<GroupStudent> _students = new();
        private readonly List<Schedule> _schedules = new();
        private readonly List<AttendanceRecord> _attendanceRecords = new();

        private StudyGroup() { } // For domain events/serialization

        public StudyGroup(string name, int subjectId, int academicYearId, int professorId, bool isActive = true) 
            : base(isActive)
        {
            ValidateName(name);
            
            Name = name;
            SubjectId = subjectId;
            AcademicYearId = academicYearId;
            ProfessorId = professorId;
        }

        // Identity properties - immutable
        public string Name { get; private init; }
        public int SubjectId { get; private init; }
        public int AcademicYearId { get; private init; }
        public int ProfessorId { get; private init; }

        // Domain references without EF Core annotations - nullable because loaded by EF Core
        public Subject? Subject { get; private set; }
        public AcademicYear? AcademicYear { get; private set; }
        public Professor? Professor { get; private set; }
        
        // Collections as readonly
        public IReadOnlyCollection<GroupStudent> Students => _students.AsReadOnly();
        public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();
        public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

        // Domain methods
        internal void AddStudent(Student student)
        {
            var groupStudent = new GroupStudent(Id, student.Id);
            _students.Add(groupStudent);
        }

        internal void RemoveStudent(int studentId)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == studentId);
            if (student != null)
                _students.Remove(student);
        }

        internal void AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Group name cannot be empty");
            if (name.Length > 100)
                throw new DomainException("Group name cannot exceed 100 characters");
        }
    }
}