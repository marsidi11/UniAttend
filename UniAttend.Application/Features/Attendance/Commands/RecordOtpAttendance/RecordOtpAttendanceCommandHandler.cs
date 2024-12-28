using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance
{
    public class RecordOtpAttendanceCommandHandler : IRequestHandler<RecordOtpAttendanceCommand, AttendanceRecordDto>
    {
        private readonly IAttendanceService _attendanceService;

        public RecordOtpAttendanceCommandHandler(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task<AttendanceRecordDto> Handle(RecordOtpAttendanceCommand request, CancellationToken cancellationToken)
        {
            var record = await _attendanceService.RecordOtpAttendanceAsync(
                request.OtpCode,
                request.StudentId,
                request.ClassId,
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