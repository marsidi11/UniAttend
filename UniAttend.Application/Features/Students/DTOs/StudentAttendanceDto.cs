namespace UniAttend.Application.Features.Students.DTOs
{
    public class StudentAttendanceDto
    {
        public int CourseId { get; init; }
        public string CourseName { get; init; } = string.Empty;
        public DateTime CheckInTime { get; init; }
        public string CheckInMethod { get; init; } = string.Empty;
        public bool IsConfirmed { get; init; }
    }
}