using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.CourseSessions.DTOs
{
    public class CourseSessionDto
    {
        public int Id { get; init; }
        public int StudyGroupId { get; init; }
        public string StudyGroupName { get; init; } = string.Empty;
        public int ClassroomId { get; init; }
        public string ClassroomName { get; init; } = string.Empty;
        public DateTime Date { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public SessionStatus Status { get; init; }
    }
}