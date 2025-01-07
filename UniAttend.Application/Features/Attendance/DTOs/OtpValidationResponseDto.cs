namespace UniAttend.Application.Features.Attendance.DTOs
{
    public class OtpValidationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public AttendanceRecordDto? AttendanceRecord { get; set; }
    }
}