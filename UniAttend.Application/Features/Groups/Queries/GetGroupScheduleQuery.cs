using MediatR;
using UniAttend.Application.Features.Groups.DTOs;

namespace UniAttend.Application.Features.Groups.Queries
{
    public record GetGroupScheduleQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public int GroupId { get; init; }
    }
}