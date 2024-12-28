namespace UniAttend.Application.Features.Attendance.DTOs
{
    /// <summary>
    /// Data transfer object representing an attendance record.
    /// </summary>
    public record AttendanceRecordDto(
        DateTime CheckInTime,
        string CheckInMethod,
        bool IsConfirmed,
        string CourseName,
        string Professor
    );
}