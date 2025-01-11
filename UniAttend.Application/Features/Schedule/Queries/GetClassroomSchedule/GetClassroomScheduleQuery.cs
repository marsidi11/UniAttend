using MediatR;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetClassroomSchedule
{
    public record GetClassroomScheduleQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public int ClassroomId { get; init; }
    }
}