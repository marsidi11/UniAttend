namespace UniAttend.Application.Features.Attendance.DTOs
{
    public record AttendanceRecordDto(
        DateTime CheckInTime,
        string CheckInMethod,
        bool IsConfirmed,
        string CourseName,
        string ProfessorName
    );
}