using MediatR;
using UniAttend.Application.Features.CourseSessions.DTOs;
using UniAttend.Core.Interfaces.Repositories;
using System.Linq;

namespace UniAttend.Application.Features.CourseSessions.Queries.GetActiveCourseSessions
{
    /// <summary>
    /// Handles the query to retrieve active course sessions.
    /// </summary>
    public class GetActiveCourseSessionsQueryHandler : IRequestHandler<GetActiveCourseSessionsQuery, IEnumerable<CourseSessionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetActiveCourseSessionsQueryHandler"/> class.
        /// </summary>
        public GetActiveCourseSessionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processes the query to retrieve active course sessions.
        /// </summary>
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