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
        public List<StudyGroupSummaryDto> Groups { get; init; } = new();
    }

    public record StudyGroupSummaryDto
    {
        public int StudyGroupId { get; init; }
        public string StudyGroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public int EnrolledStudents { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}