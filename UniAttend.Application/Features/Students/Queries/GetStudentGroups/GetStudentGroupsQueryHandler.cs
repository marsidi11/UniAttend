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
            var groups = request.AcademicYearId.HasValue
                ? await _groupStudentRepository.GetActiveGroupsByStudentIdAsync(
                    request.StudentId, 
                    request.AcademicYearId.Value, 
                    cancellationToken)
                : await _groupStudentRepository.GetByStudentIdAsync(
                    request.StudentId, 
                    cancellationToken);
        
            return groups
                .Where(g => g.Group != null) // Filter out records with null Group
                .Select(g => new StudentGroupDto
                {
                    GroupId = g.GroupId,
                    GroupName = g.Group!.Name,
                    SubjectName = g.Group.Subject?.Name ?? "Unknown Subject",
                    AcademicYear = g.Group.AcademicYear?.Name ?? "Unknown Year",
                    ProfessorName = g.Group.Professor?.User != null 
                        ? $"{g.Group.Professor.User.FirstName} {g.Group.Professor.User.LastName}"
                        : "Unknown Professor"
                });
        }
    }
}