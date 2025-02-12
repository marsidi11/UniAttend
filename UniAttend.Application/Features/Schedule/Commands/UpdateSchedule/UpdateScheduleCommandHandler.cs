using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Schedule.Commands.UpdateSchedule
{
    public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, Unit>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateScheduleCommandHandler(IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork)
        {
            _scheduleRepository = scheduleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            // First get the schedule to ensure it exists
            var schedule = await _scheduleRepository.GetByIdAsync(request.Id, cancellationToken) 
                ?? throw new NotFoundException($"Schedule with ID {request.Id} not found");
        
            // Check classroom conflicts, excluding current schedule
            var hasClassroomConflict = await _unitOfWork.Schedules.HasClassroomConflictAsync(
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                excludeScheduleId: request.Id,
                cancellationToken: cancellationToken);
        
            if (hasClassroomConflict)
                throw new ValidationException("The classroom is already booked for this time slot");
        
            // Check group conflicts, excluding current schedule
            var hasGroupConflict = await _unitOfWork.Schedules.HasGroupConflictAsync(
                request.StudyGroupId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                excludeScheduleId: request.Id,
                cancellationToken: cancellationToken);
        
            if (hasGroupConflict)
                throw new ValidationException("The group already has a class scheduled for this time slot");
        
            // Check for time conflicts, excluding current schedule
            var hasConflict = await _unitOfWork.Schedules.HasTimeConflictAsync(
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                excludeScheduleId: request.Id,
                cancellationToken: cancellationToken);
        
            if (hasConflict)
                throw new ValidationException("There is a scheduling conflict for this time slot");
        
            // Update existing schedule properties
            schedule.UpdateProperties(
                request.StudyGroupId,
                request.ClassroomId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);
        
            await _scheduleRepository.UpdateAsync(schedule, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return Unit.Value;
        }
    }
}