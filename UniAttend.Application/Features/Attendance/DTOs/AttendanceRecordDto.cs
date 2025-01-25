using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Attendance.DTOs
{
    public record AttendanceRecordDto(
        int Id,
        int CourseSessionId,
        int StudentId,
        string StudentName,
        DateTime CheckInTime,
        CheckInMethod CheckInMethod,
        bool IsConfirmed,
        DateTime? ConfirmationTime,
        int? ConfirmedByProfessorId,
        string StudyGroupName,
        string ClassroomName,
        TimeSpan SessionStartTime,
        TimeSpan SessionEndTime
    );
}