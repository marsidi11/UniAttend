using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class AttendanceRecord : Entity
    {
        // Add this protected parameterless constructor for EF Core
        protected AttendanceRecord() { }
    
        public AttendanceRecord(int courseId, int studentId, DateTime checkInTime, string checkInMethod)
        {
            CourseId = courseId;
            StudentId = studentId;
            CheckInTime = checkInTime;
            CheckInMethod = checkInMethod ?? throw new ArgumentNullException(nameof(checkInMethod));
        }
    
        // Change these to { get; set; } to allow EF Core to set values
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime CheckInTime { get; set; }
        public string CheckInMethod { get; set; }
        public bool IsConfirmed { get; set; } = false;
    
        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}