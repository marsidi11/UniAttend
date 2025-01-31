using MediatR;
using UniAttend.Application.Features.CourseSessions.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.CourseSessions.Queries.GetActiveCourseSessions
{
    public class GetActiveCourseSessionsQueryHandler : IRequestHandler<GetActiveCourseSessionsQuery, IEnumerable<CourseSessionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetActiveCourseSessionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseSessionDto>> Handle(
            GetActiveCourseSessionsQuery request, 
            CancellationToken cancellationToken)
        {
            var sessions = await _unitOfWork.CourseSessions.GetActiveSessionsAsync(
                courseSessionId: request.CourseSessionId,
                studyGroupId: request.StudyGroupId,
                classroomId: request.ClassroomId,
                professorId: request.ProfessorId,
                date: request.Date,
                cancellationToken: cancellationToken);
        
            return sessions.Select(s => new CourseSessionDto
            {
                Id = s.Id,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.Name,
                ClassroomId = s.ClassroomId,
                ClassroomName = s.Classroom.Name,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status
            });
        }
    }
}