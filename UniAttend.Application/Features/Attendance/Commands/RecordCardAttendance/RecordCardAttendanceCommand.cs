using MediatR;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Commands.RecordCardAttendance
{
    public class RecordCardAttendanceCommand : IRequest<AttendanceRecordDto>
    {
        public string CardId { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public int ClassId { get; set; }
    }
}