using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class StudyGroup : ActiveEntity
    {
        private readonly List<GroupStudent> _students = new();
        private readonly List<Schedule> _schedules = new();
        private readonly List<CourseSession> _courseSessions = new();
        private readonly List<AttendanceRecord> _attendanceRecords = new();
        private string _name;
        private int _subjectId;
        private int _professorId;

        private StudyGroup() { } // For domain events/serialization

        public StudyGroup(string name, int subjectId, int academicYearId, int professorId, bool isActive = true)
            : base(isActive)
        {
            ValidateName(name);

            _name = name;
            _subjectId = subjectId;
            AcademicYearId = academicYearId;
            _professorId = professorId;
        }

        public string Name => _name;
        public int SubjectId => _subjectId;
        public int AcademicYearId { get; private init; }
        public int ProfessorId => _professorId;

        // Domain references without EF Core annotations - nullable because loaded by EF Core
        public Subject? Subject { get; private set; }
        public AcademicYear? AcademicYear { get; private set; }
        public Professor? Professor { get; private set; }

        // Collections as readonly
        public IReadOnlyCollection<GroupStudent> Students => _students.AsReadOnly();
        public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();
        public IReadOnlyCollection<CourseSession> CourseSessions => _courseSessions.AsReadOnly();
        public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

        // Domain methods
        internal void AddStudent(Student student)
        {
            var groupStudent = new GroupStudent(Id, student.Id);
            _students.Add(groupStudent);
        }

        public void RemoveStudent(int studentId)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == studentId);
            if (student != null)
                _students.Remove(student);
        }

        public void Update(string name, int subjectId, int professorId)
        {
            ValidateName(name);
            _name = name;
            _subjectId = subjectId;
            _professorId = professorId;
        }

        internal void AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
        }

        public void AddCourseSession(CourseSession courseSession)
        {
            if (courseSession == null)
                throw new ArgumentNullException(nameof(courseSession));

            _courseSessions.Add(courseSession);
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("StudyGroup name cannot be empty");
            if (name.Length > 100)
                throw new DomainException("StudyGroup name cannot exceed 100 characters");
        }

        public void Activate() => SetActive(true);
        public void Deactivate() => SetActive(false);
    }
}