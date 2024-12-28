using MediatR;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries
{
    public class GetStudentAttendanceQuery : IRequest<IEnumerable<AttendanceRecordDto>>
    {
        public int StudentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}