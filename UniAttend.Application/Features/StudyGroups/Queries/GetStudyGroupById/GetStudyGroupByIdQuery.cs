using MediatR;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroupById
{
    public record GetStudyGroupByIdQuery : IRequest<StudyGroupDto?>
    {
        public int Id { get; init; }
    }
}