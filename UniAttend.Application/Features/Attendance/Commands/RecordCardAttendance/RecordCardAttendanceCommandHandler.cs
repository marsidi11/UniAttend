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
                record.CheckInTime,
                record.CheckInMethod,
                record.IsConfirmed,
                "Course Name",
                "Professor Name"
            );
        }
    }
}