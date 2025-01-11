using MediatR;

namespace UniAttend.Application.Features.Schedule.Commands.DeleteSchedule
{
    public record DeleteScheduleCommand : IRequest<Unit>
    {
        public int Id { get; init; }
    }
}