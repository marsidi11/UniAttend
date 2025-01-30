using MediatR;

namespace UniAttend.Application.Features.Attendance.Commands.MarkAbsent
{
    public class MarkAbsentCommand : IRequest<Unit>
    {
        public int CourseSessionId { get; set; }
        public int StudentId { get; set; }
    }
}