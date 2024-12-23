using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class AttendanceRecord : Entity
    {
        public AttendanceRecord(int classId, int studentId, DateTime checkInTime, string checkInMethod)
        {
            ClassId = classId;
            StudentId = studentId;
            CheckInTime = checkInTime;
            CheckInMethod = checkInMethod ?? throw new ArgumentNullException(nameof(checkInMethod));
        }

        public int ClassId { get; }
        public int StudentId { get; }
        public DateTime CheckInTime { get; }
        public string CheckInMethod { get; }
        public bool IsConfirmed { get; set; } = false;
    }
}