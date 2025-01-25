using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetAllSchedules
{
    public class GetAllSchedulesQueryHandler : IRequestHandler<GetAllSchedulesQuery, IEnumerable<ScheduleDto>>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public GetAllSchedulesQueryHandler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<ScheduleDto>> Handle(
            GetAllSchedulesQuery request,
            CancellationToken cancellationToken)
        {
            var schedules = await _scheduleRepository.GetAllWithDetailsAsync(cancellationToken);

            return schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup?.Name ?? string.Empty,
                ClassroomId = s.ClassroomId,
                ClassroomName = s.Classroom?.Name ?? string.Empty,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                SubjectName = s.StudyGroup?.Subject?.Name ?? string.Empty,
                ProfessorName = $"{s.StudyGroup?.Professor?.User?.FirstName} {s.StudyGroup?.Professor?.User?.LastName}".Trim()
            });
        }
    }
}