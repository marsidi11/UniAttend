namespace UniAttend.Application.Features.Reports.DTOs
{
    public record AcademicYearReportDto
    {
        public int AcademicYearId { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int TotalStudents { get; init; }
        public int TotalGroups { get; init; }
        public int ActiveGroups { get; init; }
        public decimal OverallAttendance { get; init; }
        public int PendingAttendanceConfirmations { get; init; }
    }
}