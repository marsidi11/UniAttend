namespace UniAttend.Core.Entities.Stats
{
    public class AttendanceStats
    {
        public int TotalCourseSessions { get; set; }
        public int AttendedCourseSessions { get; set; }
        public decimal AttendanceRate { get; set; }
        public decimal AverageAttendance { get; set; }
        public int TotalSessions { get; set; }        
        public int TotalStudents { get; set; }
        public int PresentCount { get; set; }         
        public int AbsentCount { get; set; }           
        public int PendingCount { get; set; } 
        public int PendingConfirmations { get; set; }
    }
}