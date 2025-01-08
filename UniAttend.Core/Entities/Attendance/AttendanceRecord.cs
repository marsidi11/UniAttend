using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class AttendanceRecord : Entity  
    {
        private AttendanceRecord() { } // For reflection/serialization
    
        public AttendanceRecord(int courseId, int studentId, DateTime checkInTime, string checkInMethod)
        {
            CourseId = courseId;
            StudentId = studentId;
            CheckInTime = checkInTime;
            CheckInMethod = checkInMethod ?? throw new ArgumentNullException(nameof(checkInMethod));
            IsConfirmed = false;
        }
    
        // Identity properties - immutable
        public int CourseId { get; }
        public int StudentId { get; }
        public DateTime CheckInTime { get; }
        public string CheckInMethod { get; }
        public bool IsConfirmed { get; private set; }
    
        // Navigation properties - nullable for EF Core
        public Course? Course { get; private set; }
        public Student? Student { get; private set; }
    
        // Domain methods
        public void Confirm() => IsConfirmed = true;
    }
}