using MediatR;
using UniAttend.Application.Features.Classes.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Classes.Queries.GetActiveClasses
{
    public class GetActiveClassesQueryHandler : IRequestHandler<GetActiveClassesQuery, IEnumerable<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetActiveClassesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClassDto>> Handle(GetActiveClassesQuery request, CancellationToken cancellationToken)
        {
            var sessions = await _unitOfWork.CourseSessions.GetActiveSessionsAsync(
                studyGroupId: request.StudyGroupId,
                classroomId: request.ClassroomId,
                date: request.Date,
                cancellationToken: cancellationToken);

            return sessions.Select(s => new ClassDto
            {
                Id = s.Id,
                StudyGroupId = s.StudyGroupId,
                GroupName = s.StudyGroup.Name,
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