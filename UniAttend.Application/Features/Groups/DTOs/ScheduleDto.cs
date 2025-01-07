namespace UniAttend.Application.Features.Groups.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; init; }
        public int DayOfWeek { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public string ClassroomName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string ProfessorName { get; init; } = string.Empty;
    }
}