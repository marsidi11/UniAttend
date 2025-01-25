using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities.Attendance
{
    public class CourseSession : ActiveEntity
    {
        private readonly List<AttendanceRecord> _attendanceRecords = new();

        public CourseSession(int studyGroupId, int classroomId, DateTime date, TimeSpan startTime, TimeSpan endTime, string status)
        {
            StudyGroupId = studyGroupId;
            ClassroomId = classroomId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }

        public int StudyGroupId { get; private set; }
        public int ClassroomId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public string Status { get; private set; }

        // Navigation properties
        public virtual StudyGroup StudyGroup { get; private set; } = null!;
        public virtual Classroom Classroom { get; private set; } = null!;
        public virtual IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

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