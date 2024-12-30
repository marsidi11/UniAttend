using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class CourseSession : Entity
    {
        public CourseSession(int courseId, int groupId, int classroomId, DateTime date, TimeSpan startTime, TimeSpan endTime, string status)
        {
            CourseId = courseId;
            GroupId = groupId;
            ClassroomId = classroomId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        public int CourseId { get; }
        public int GroupId { get; }
        public int ClassroomId { get; }
        public DateTime Date { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Status { get; }

        // Navigation properties
        public virtual Course Course { get; init; } = null!;
        public virtual StudyGroup Group { get; init; } = null!;
        public virtual Classroom Classroom { get; init; } = null!;
    }
}