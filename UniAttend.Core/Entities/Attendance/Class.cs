using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class Class : Entity
    {
        public Class(int groupId, int classroomId, DateTime date, TimeSpan startTime, TimeSpan endTime, string status)
        {
            GroupId = groupId;
            ClassroomId = classroomId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        public int GroupId { get; }
        public int ClassroomId { get; }
        public DateTime Date { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Status { get; }
    }
}