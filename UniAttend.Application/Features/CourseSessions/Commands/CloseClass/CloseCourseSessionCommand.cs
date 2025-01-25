using MediatR;

namespace UniAttend.Application.Features.CourseSessions.Commands.CloseCourseSession
{
    public record CloseCourseSessionCommand : IRequest<Unit>
    {
        public int CourseSessionId { get; init; }
    }
}