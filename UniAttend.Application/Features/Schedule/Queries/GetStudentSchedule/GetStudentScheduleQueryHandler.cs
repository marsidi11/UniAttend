using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetStudentSchedule
{
    public class GetStudentScheduleQueryHandler 
        : IRequestHandler<GetStudentScheduleQuery, IEnumerable<ScheduleDto>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;

        public GetStudentScheduleQueryHandler(
            IScheduleRepository scheduleRepository,
            IGroupStudentRepository groupStudentRepository)
        {
            _scheduleRepository = scheduleRepository;
            _groupStudentRepository = groupStudentRepository;
        }

        public async Task<IEnumerable<ScheduleDto>> Handle(
            GetStudentScheduleQuery request, 
            CancellationToken cancellationToken)
        {
            // Get all groups for the student
            var studentGroups = await _groupStudentRepository.GetByStudentIdAsync(
                request.StudentId, 
                cancellationToken);

            var groupIds = studentGroups.Select(g => g.StudyGroupId).ToList();

            // Get schedules for all student's groups
            var schedules = await _scheduleRepository.GetAllWithDetailsAsync(cancellationToken);
            var studentSchedules = schedules.Where(s => groupIds.Contains(s.StudyGroupId));

            return studentSchedules.Select(s => new ScheduleDto
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
                ProfessorName = s.StudyGroup?.Professor?.User != null
                    ? $"{s.StudyGroup.Professor.User.FirstName} {s.StudyGroup.Professor.User.LastName}".Trim()
                    : string.Empty
            });
        }
    }
}