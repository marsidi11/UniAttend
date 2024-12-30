using UniAttend.Core.Interfaces.Repositories;
using ScheduleEntity = UniAttend.Core.Entities.Schedule;
using MediatR;
using FluentValidation;

namespace UniAttend.Application.Features.Schedule.Commands.CreateSchedule
{
    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateScheduleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            // Check for time conflicts
            var hasConflict = await _unitOfWork.Schedules.HasTimeConflictAsync(
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                cancellationToken: cancellationToken);

            if (hasConflict)
                throw new ValidationException("There is a scheduling conflict for this time slot");

            var schedule = new ScheduleEntity(
                request.GroupId,
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);

            await _unitOfWork.Schedules.AddAsync(schedule, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return schedule.Id;
        }
    }
}