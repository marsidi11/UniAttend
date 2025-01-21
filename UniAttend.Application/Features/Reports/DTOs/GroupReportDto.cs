namespace UniAttend.Application.Features.Reports.DTOs
{
    public record GroupReportDto
    {
        public int StudyGroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string ProfessorName { get; init; } = string.Empty;
        public int TotalStudents { get; init; }
        public int TotalClasses { get; init; }
        public decimal AverageAttendance { get; init; }
        public List<StudentAttendanceDto> Students { get; init; } = new();
    }

    public record StudentAttendanceDto
    {
        public int StudentId { get; init; }
        public string StudentNumber { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public int AttendedClasses { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}