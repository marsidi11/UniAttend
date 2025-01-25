using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetProfessorSchedule
{
    public class GetProfessorScheduleQueryHandler 
        : IRequestHandler<GetProfessorScheduleQuery, IEnumerable<ScheduleDto>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;

        public GetProfessorScheduleQueryHandler(
            IScheduleRepository scheduleRepository,
            IStudyGroupRepository studyGroupRepository)
        {
            _scheduleRepository = scheduleRepository;
            _studyGroupRepository = studyGroupRepository;
        }

        public async Task<IEnumerable<ScheduleDto>> Handle(
            GetProfessorScheduleQuery request, 
            CancellationToken cancellationToken)
        {
            // Get all groups for the professor
            var professorGroups = await _studyGroupRepository.GetByProfessorIdAsync(
                request.ProfessorId, 
                cancellationToken: cancellationToken);

            var groupIds = professorGroups.Select(g => g.Id).ToList();

            // Get schedules for all professor's groups
            var schedules = await _scheduleRepository.GetAllWithDetailsAsync(cancellationToken);
            var professorSchedules = schedules.Where(s => groupIds.Contains(s.StudyGroupId));

            // Map to DTOs
            return professorSchedules.Select(s => new ScheduleDto
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
                ProfessorName = $"{s.StudyGroup?.Professor?.User?.FirstName} {s.StudyGroup?.Professor?.User?.LastName}".Trim(),
            })
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime);
        }
    }
}