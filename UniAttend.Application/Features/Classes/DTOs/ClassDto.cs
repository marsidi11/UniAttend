namespace UniAttend.Application.Features.Classes.DTOs
{
    public class ClassDto
    {
        public int Id { get; init; }
        public int GroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public int ClassroomId { get; init; }
        public string ClassroomName { get; init; } = string.Empty;
        public DateTime Date { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}