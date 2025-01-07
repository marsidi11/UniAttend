namespace UniAttend.Application.Features.Groups.DTOs
{
    public class AttendanceStatsDto
    {
        public int TotalStudents { get; init; }
        public int TotalClasses { get; init; }
        public decimal AverageAttendance { get; init; }
        public int PresentToday { get; init; }
        public int AbsentToday { get; init; }
    }
}