using MediatR;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries.GetCourseSessionAttendance
{
    public class GetCourseSessionAttendanceQuery : IRequest<IEnumerable<AttendanceRecordDto>>
    {
        public int CourseSessionId { get; set; }
        public DateTime? Date { get; set; }
    }
}