using MediatR;
using UniAttend.Application.Features.CourseSessions.DTOs;

namespace UniAttend.Application.Features.CourseSessions.Queries.GetActiveCourseSessions
{
    public record GetActiveCourseSessionsQuery : IRequest<IEnumerable<CourseSessionDto>>
    {
        public int? CourseSessionId { get; init; }
        public int? StudyGroupId { get; init; }
        public int? ClassroomId { get; init; }
        public DateTime? Date { get; init; }
    }
}