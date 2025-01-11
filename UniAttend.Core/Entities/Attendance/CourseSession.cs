using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities.Attendance
{
    public class CourseSession : Entity
    {
        public int GroupId { get; private set; }
        public int ClassroomId { get; private set; }
        public int CourseId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public string Status { get; private set; }

        // Navigation properties
        public StudyGroup Group { get; private set; }
        public Classroom Classroom { get; private set; }
        public Course Course { get; private set; }
        private readonly List<AttendanceRecord> _attendanceRecords = new();
        public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

        private CourseSession() { } // For EF

        public CourseSession(int groupId, int classroomId, int courseId, DateTime date, 
            TimeSpan startTime, TimeSpan endTime, string status)
        {
            GroupId = groupId;
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