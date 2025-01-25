namespace UniAttend.Application.Features.Reports.DTOs
{
    public record AttendanceReportDto
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int TotalStudents { get; init; }
        public int TotalCourseSessions { get; init; }
        public decimal OverallAttendance { get; init; }
        public List<DailyAttendanceDto> DailyRecords { get; init; } = new();
    }

    public record DailyAttendanceDto
    {
        public DateTime Date { get; init; }
        public int TotalCourseSessions { get; init; }
        public int PresentStudents { get; init; }
        public int AbsentStudents { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}