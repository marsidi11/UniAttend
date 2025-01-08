using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class CourseSession : Entity
    {
        private CourseSession() { } // For domain events/serialization only
    
        public CourseSession(int courseId, int groupId, int classroomId, DateTime date, 
            TimeSpan startTime, TimeSpan endTime, string status)
        {
            ValidateTimeRange(startTime, endTime);
            ValidateStatus(status);
            
            CourseId = courseId;
            GroupId = groupId;
            ClassroomId = classroomId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }
    
        // Identity properties - immutable
        public int CourseId { get; }
        public int GroupId { get; }
        public int ClassroomId { get; }
        public DateTime Date { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Status { get; private set; }
    
        // Domain references without EF Core annotations - nullable for EF Core
        public Course? Course { get; private set; }
        public StudyGroup? Group { get; private set; }
        public Classroom? Classroom { get; private set; }
    
        // Domain methods
        public void UpdateStatus(string newStatus)
        {
            ValidateStatus(newStatus);
            Status = newStatus;
        }
    
        // Domain validation
        private void ValidateTimeRange(TimeSpan start, TimeSpan end)
        {
            if (start >= end)
                throw new DomainException("Start time must be before end time");
        }
    
        private void ValidateStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new DomainException("Status cannot be empty");
        }
    }
}