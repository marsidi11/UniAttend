namespace UniAttend.Application.Features.Reports.DTOs
{
    public record DepartmentReportDto
    {
        public int DepartmentId { get; init; }
        public string DepartmentName { get; init; } = string.Empty;
        public int TotalStudents { get; init; }
        public int TotalGroups { get; init; }
        public int TotalSubjects { get; init; }
        public decimal AverageAttendance { get; init; }
        public List<GroupSummaryDto> Groups { get; init; } = new();
    }

    public record GroupSummaryDto
    {
        public int GroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public int EnrolledStudents { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}