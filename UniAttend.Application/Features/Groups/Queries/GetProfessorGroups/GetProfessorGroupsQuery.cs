using MediatR;
using UniAttend.Application.Features.Groups.DTOs;

namespace UniAttend.Application.Features.Groups.Queries.GetProfessorGroups
{
    public record GetProfessorGroupsQuery : IRequest<IEnumerable<StudyGroupDto>>
    {
        public int ProfessorId { get; init; }
        public int? AcademicYearId { get; init; }
    }
}