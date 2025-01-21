using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetClassroomSchedule
{
    public class GetClassroomScheduleQueryHandler : IRequestHandler<GetClassroomScheduleQuery, IEnumerable<ScheduleDto>>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public GetClassroomScheduleQueryHandler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<ScheduleDto>> Handle(GetClassroomScheduleQuery request, CancellationToken cancellationToken)
        {
            var schedules = await _scheduleRepository.GetByClassroomIdAsync(request.ClassroomId, cancellationToken);

            return schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                StudyGroupId = s.StudyGroupId,
                GroupName = s.StudyGroup?.Name ?? string.Empty,
                ClassroomId = s.ClassroomId,
                ClassroomName = s.Classroom?.Name ?? string.Empty,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                SubjectName = s.StudyGroup?.Subject?.Name ?? string.Empty,
                ProfessorName = $"{s.StudyGroup?.Professor?.User?.FirstName} {s.StudyGroup?.Professor?.User?.LastName}".Trim(),
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });
        }
    }
}