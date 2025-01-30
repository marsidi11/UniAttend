using MediatR;

namespace UniAttend.Application.Features.Attendance.Commands.ConfirmAttendance
{
    public class ConfirmAttendanceCommand : IRequest<Unit>
    {
        public int CourseSessionId { get; set; }
    }
}