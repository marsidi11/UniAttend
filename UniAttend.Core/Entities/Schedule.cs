using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Schedule : Entity
    {
        private Schedule() { } // For domain events/serialization

        public Schedule(int studyGroupId, int classroomId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            ValidateTimeRange(startTime, endTime);
            ValidateDayOfWeek(dayOfWeek);

            StudyGroupId = studyGroupId;
            ClassroomId = classroomId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }

        public void UpdateProperties(
        int studyGroupId,
        int classroomId,
        int dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime)
        {
            ValidateTimeRange(startTime, endTime);
            ValidateDayOfWeek(dayOfWeek);

            StudyGroupId = studyGroupId;
            ClassroomId = classroomId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }

        // Identity properties with private setters
        public int StudyGroupId { get; private set; }
        public int ClassroomId { get; private set; }
        public int DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        // Navigation properties
        public StudyGroup StudyGroup { get; private set; }
        public Classroom Classroom { get; private set; }

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