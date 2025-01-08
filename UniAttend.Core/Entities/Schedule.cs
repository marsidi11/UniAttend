using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Schedule : Entity
    {
        private Schedule() { } // For domain events/serialization

        public Schedule(int groupId, int classroomId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            ValidateTimeRange(startTime, endTime);
            ValidateDayOfWeek(dayOfWeek);
            
            GroupId = groupId;
            ClassroomId = classroomId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }

        // Identity properties - immutable
        public int GroupId { get; }
        public int ClassroomId { get; }
        public int DayOfWeek { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        // Domain references without EF Core annotations
        public StudyGroup Group { get; }
        public Classroom Classroom { get; }

        // Domain validation
        private void ValidateTimeRange(TimeSpan start, TimeSpan end)
        {
            if (start >= end)
                throw new DomainException("Start time must be before end time");
        }

        private void ValidateDayOfWeek(int dayOfWeek)
        {
            if (dayOfWeek < 1 || dayOfWeek > 7)
                throw new DomainException("Day of week must be between 1 and 7");
        }
    }
}