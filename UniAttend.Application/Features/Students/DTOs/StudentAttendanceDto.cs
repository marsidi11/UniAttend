using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Students.DTOs
{
    public class StudentAttendanceDto
    {
        public int CourseSessionId { get; init; }
        public string StudyGroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string ClassroomName { get; init; } = string.Empty;
        public DateTime CheckInTime { get; init; }
        public CheckInMethod CheckInMethod { get; init; }
        public bool IsConfirmed { get; init; }
        public DateTime? ConfirmationTime { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}