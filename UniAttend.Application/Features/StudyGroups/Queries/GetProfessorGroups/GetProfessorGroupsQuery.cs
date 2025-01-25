using MediatR;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetProfessorStudyGroups
{
    public record GetProfessorStudyGroupsQuery : IRequest<IEnumerable<StudyGroupDto>>
    {
        public int ProfessorId { get; init; }
        public int? AcademicYearId { get; init; }
    }
}