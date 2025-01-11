namespace UniAttend.Application.Features.Reports.DTOs
{
    public record StudentReportDto
    {
        public int StudentId { get; init; }
        public string StudentNumber { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string DepartmentName { get; init; } = string.Empty;
        public string CardId { get; init; } = string.Empty;
        public int TotalAttendance { get; init; }
        public int TotalClasses { get; init; }
        public decimal AttendanceRate { get; init; }
        public List<SubjectAttendanceDto> Subjects { get; init; } = new();
    }

    public record SubjectAttendanceDto
    {
        public int SubjectId { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public string GroupName { get; init; } = string.Empty;
        public int AttendedClasses { get; init; }
        public int TotalClasses { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}