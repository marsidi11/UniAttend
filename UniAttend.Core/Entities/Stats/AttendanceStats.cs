namespace UniAttend.Core.Entities.Stats
{
    public class AttendanceStats
    {
        public int TotalClasses { get; set; }
        public int AttendedClasses { get; set; }
        public decimal AverageAttendance { get; set; }
        public decimal AttendanceRate { get; set; }
        public int? AbsentStudents { get; set; }
        public int? TotalStudents { get; set; }
        public int? PresentToday { get; set; }
        public int PendingConfirmations { get; set; }
    }
}