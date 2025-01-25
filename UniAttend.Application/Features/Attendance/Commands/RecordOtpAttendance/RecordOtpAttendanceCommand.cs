using MediatR;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance
{
    public class RecordOtpAttendanceCommand : IRequest<AttendanceRecordDto>
    {
        public string OtpCode { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public int CourseSessionId { get; set; }
        public VerificationType VerificationType { get; set; }
    }
}