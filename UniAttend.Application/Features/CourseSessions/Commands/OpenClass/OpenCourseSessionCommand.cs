using MediatR;
using UniAttend.Application.Features.CourseSessions.DTOs;

namespace UniAttend.Application.Features.CourseSessions.Commands.OpenCourseSession
{
    public class OpenCourseSessionCommand : IRequest<CourseSessionDto>
    {
        public int StudyGroupId { get; set; }
        public int ClassroomId { get; set; }
        public int CourseSessionId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}