using MediatR;

namespace UniAttend.Application.Features.Schedule.Commands.UpdateSchedule
{
    public record UpdateScheduleCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public int StudyGroupId { get; init; }
        public int ClassroomId { get; init; }
        public int DayOfWeek { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}