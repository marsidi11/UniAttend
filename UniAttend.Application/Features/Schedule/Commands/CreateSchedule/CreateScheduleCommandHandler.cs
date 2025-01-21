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
            // Check classroom conflicts
            var hasClassroomConflict = await _unitOfWork.Schedules.HasClassroomConflictAsync(
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                cancellationToken: cancellationToken);

            if (hasClassroomConflict)
                throw new ValidationException("The classroom is already booked for this time slot");

            // Check group conflicts
            var hasGroupConflict = await _unitOfWork.Schedules.HasGroupConflictAsync(
                request.StudyGroupId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                cancellationToken: cancellationToken);

            if (hasGroupConflict)
                throw new ValidationException("The group already has a class scheduled for this time slot");

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
                request.StudyGroupId,
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