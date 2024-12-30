using MediatR;

namespace UniAttend.Application.Features.Schedule.Commands.CreateSchedule
{
    public record CreateScheduleCommand : IRequest<int>
    {
        public int GroupId { get; init; }
        public int ClassroomId { get; init; }
        public int DayOfWeek { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}