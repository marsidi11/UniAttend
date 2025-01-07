namespace UniAttend.Application.Features.Groups.DTOs
{
    public class GroupStudentDto
    {
        public int StudentId { get; init; }
        public string StudentName { get; init; } = string.Empty;
        public string StudentNumber { get; init; } = string.Empty;
        public decimal AttendanceRate { get; init; }
        public bool IsActive { get; init; }
    }
}