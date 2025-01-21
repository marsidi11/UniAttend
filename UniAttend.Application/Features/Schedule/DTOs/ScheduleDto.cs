using System;

namespace UniAttend.Application.Features.Schedule.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; init; }
        public int StudyGroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public int ClassroomId { get; init; }
        public string ClassroomName { get; init; } = string.Empty;
        public int DayOfWeek { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public string ProfessorName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}