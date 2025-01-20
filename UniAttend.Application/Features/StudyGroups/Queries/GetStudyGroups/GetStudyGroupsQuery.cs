using MediatR;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroups
{
    public record GetStudyGroupsQuery : IRequest<IEnumerable<StudyGroupDto>>
    {
        public int? AcademicYearId { get; init; }
    }
}