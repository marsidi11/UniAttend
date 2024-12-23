using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    public class Schedule : Entity
    {
        private Schedule() { } // For EF Core

        public Schedule(int groupId, int classroomId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (dayOfWeek < 1 || dayOfWeek > 7)
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time");

            GroupId = groupId;
            ClassroomId = classroomId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int GroupId { get; private set; }
        public int ClassroomId { get; private set; }
        public int DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        // Navigation properties
        public StudyGroup Group { get; private set; }
        public Classroom Classroom { get; private set; }
    }
}