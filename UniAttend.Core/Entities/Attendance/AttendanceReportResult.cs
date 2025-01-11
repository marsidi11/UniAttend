namespace UniAttend.Core.Entities.Attendance
{
    public class AttendanceReportResult
    {
        public decimal OverallAttendance { get; init; }
        public int PendingConfirmations { get; init; }
        public int TotalRecords { get; init; }
        public int TotalClasses { get; init; }
        public decimal AverageAttendance { get; init; }
    }
}