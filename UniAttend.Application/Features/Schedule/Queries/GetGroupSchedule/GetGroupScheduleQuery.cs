using MediatR;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetGroupSchedule
{
    public record GetGroupScheduleQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public int StudyGroupId { get; init; }
    }
}