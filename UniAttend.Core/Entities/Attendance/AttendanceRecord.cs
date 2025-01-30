using UniAttend.Core.Entities.Base;
using UniAttend.Core.Enums;

namespace UniAttend.Core.Entities.Attendance
{
    public class AttendanceRecord : Entity
    {
        private AttendanceRecord() { } // For EF Core

        public AttendanceRecord(int courseSessionId, int studentId, DateTime checkInTime, CheckInMethod checkInMethod)
        {
            CourseSessionId = courseSessionId;
            StudentId = studentId;
            CheckInTime = checkInTime;
            CheckInMethod = checkInMethod;
            IsConfirmed = false;
            Status = AttendanceStatus.PendingConfirmation;
        }

        public int CourseSessionId { get; private set; }
        public int StudentId { get; private set; }
        public DateTime CheckInTime { get; private set; }
        public CheckInMethod CheckInMethod { get; private set; }
        public bool IsConfirmed { get; private set; }
        public DateTime? ConfirmationTime { get; private set; }
        public bool IsAbsent { get; private set; }
        public AttendanceStatus Status { get; private set; }

        // Navigation properties
        public virtual CourseSession CourseSession { get; private set; } = null!;
        public virtual Student Student { get; private set; } = null!;

        public void Confirm()
        {
            IsConfirmed = true;
            Status = AttendanceStatus.Present;
            ConfirmationTime = DateTime.UtcNow;
        }

        public void MarkAsAbsent()
        {
            IsAbsent = true;
            IsConfirmed = true;
            Status = AttendanceStatus.Absent;
            ConfirmationTime = DateTime.UtcNow;
        }
    }
}