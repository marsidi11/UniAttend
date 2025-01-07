using MediatR;
using UniAttend.Application.Features.Groups.DTOs;

namespace UniAttend.Application.Features.Groups.Queries
{
    public record GetGroupStudentsQuery : IRequest<IEnumerable<GroupStudentDto>>
    {
        public int GroupId { get; init; }
    }
}