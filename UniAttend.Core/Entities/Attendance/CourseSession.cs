using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities.Attendance
{
    public class CourseSession : Entity
    {
        public int StudyGroupId { get; private set; }
        public int ClassroomId { get; private set; }
        public int CourseId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public string Status { get; private set; }

        // Navigation properties
        public StudyGroup StudyGroup { get; private set; }
        public Classroom Classroom { get; private set; }
        public Course Course { get; private set; }
        private readonly List<AttendanceRecord> _attendanceRecords = new();
        public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

        private CourseSession() { } // For EF

        public CourseSession(int studyGroupId, int classroomId, int courseId, DateTime date, 
            TimeSpan startTime, TimeSpan endTime, string status)
        {
            StudyGroupId = studyGroupId;
            ClassroomId = classroomId;
            CourseId = courseId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void AddAttendanceRecord(AttendanceRecord record)
        {
            _attendanceRecords.Add(record);
        }
    }
}