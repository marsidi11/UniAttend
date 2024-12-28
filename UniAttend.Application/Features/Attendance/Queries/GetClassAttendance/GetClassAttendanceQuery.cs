using MediatR;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries
{
    public class GetClassAttendanceQuery : IRequest<IEnumerable<AttendanceRecordDto>>
    {
        public int ClassId { get; set; }
        public DateTime? Date { get; set; }
    }
}