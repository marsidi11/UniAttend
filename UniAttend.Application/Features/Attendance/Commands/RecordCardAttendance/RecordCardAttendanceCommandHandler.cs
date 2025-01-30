using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Commands.RecordCardAttendance
{
    public class RecordCardAttendanceCommandHandler : IRequestHandler<RecordCardAttendanceCommand, AttendanceRecordDto>
    {
        private readonly IAttendanceService _attendanceService;

        public RecordCardAttendanceCommandHandler(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task<AttendanceRecordDto> Handle(RecordCardAttendanceCommand request, CancellationToken cancellationToken)
        {
            var record = await _attendanceService.RecordCardAttendanceAsync(
                request.CardId,
                request.DeviceId,
                cancellationToken);

            return new AttendanceRecordDto(
                record.Id,
                record.CourseSessionId,
                record.StudentId,
                $"{record.Student?.User?.FirstName} {record.Student?.User?.LastName}",
                record.CheckInTime,
                record.CheckInMethod,
                record.IsConfirmed,
                record.IsAbsent,
                record.ConfirmationTime,
                record.CourseSession.StudyGroup.Name,
                record.CourseSession.Classroom.Name,
                record.CourseSession.StartTime,
                record.CourseSession.EndTime
            );
        }
    }
}