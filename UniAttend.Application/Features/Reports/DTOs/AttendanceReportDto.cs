namespace UniAttend.Application.Features.Reports.DTOs
{
    public record AttendanceReportDto
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string Department { get; init; } = string.Empty;
        public int TotalStudents { get; init; }
        public int TotalClasses { get; init; }
        public decimal AverageAttendance { get; init; }
        public IEnumerable<AttendanceByGroupDto> AttendanceByGroups { get; init; } = 
            Array.Empty<AttendanceByGroupDto>();
    }

    public record AttendanceByGroupDto
    {
        public int GroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public string Subject { get; init; } = string.Empty;
        public int EnrolledStudents { get; init; }
        public decimal AttendancePercentage { get; init; }
    }
}