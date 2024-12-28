using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class AttendanceRecord : Entity
    {
        public AttendanceRecord(int courseId, int studentId, DateTime checkInTime, string checkInMethod)
        {
            CourseId = courseId;
            StudentId = studentId;
            CheckInTime = checkInTime;
            CheckInMethod = checkInMethod ?? throw new ArgumentNullException(nameof(checkInMethod));
        }

        public int CourseId { get; }
        public int StudentId { get; }
        public DateTime CheckInTime { get; }
        public string CheckInMethod { get; }
        public bool IsConfirmed { get; set; } = false;
    }
}