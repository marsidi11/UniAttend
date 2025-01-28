using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentGroups
{
    public class GetStudentGroupsQueryHandler 
        : IRequestHandler<GetStudentGroupsQuery, IEnumerable<StudentGroupDto>>
    {
        private readonly IGroupStudentRepository _groupStudentRepository;

        public GetStudentGroupsQueryHandler(IGroupStudentRepository groupStudentRepository)
        {
            _groupStudentRepository = groupStudentRepository;
        }

                public async Task<IEnumerable<StudentGroupDto>> Handle(
            GetStudentGroupsQuery request, 
            CancellationToken cancellationToken)
        {
            var studyGroups = await _groupStudentRepository.GetByStudentIdAsync(
                request.StudentId, 
                cancellationToken);
        
            return studyGroups
                .Where(g => g.StudyGroup != null)
                .Select(g => new StudentGroupDto
                {
                    StudyGroupId = g.StudyGroupId,
                    StudyGroupName = g.StudyGroup!.Name,
                    SubjectName = g.StudyGroup.Subject?.Name ?? "Unknown Subject",
                    AcademicYearName = g.StudyGroup.AcademicYear?.Name ?? "Unknown Year",
                    ProfessorName = g.StudyGroup.Professor?.User != null 
                        ? $"{g.StudyGroup.Professor.User.FirstName} {g.StudyGroup.Professor.User.LastName}" 
                        : "Unknown Professor"
                });
        }
    }
}