using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroups
{
    public class GetStudyGroupsQueryHandler : IRequestHandler<GetStudyGroupsQuery, IEnumerable<StudyGroupDto>>
    {
        private readonly IStudyGroupRepository _groupRepository;

        public GetStudyGroupsQueryHandler(IStudyGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<StudyGroupDto>> Handle(GetStudyGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAllWithDetailsAsync(request.AcademicYearId, cancellationToken);

            return groups.Select(g => new StudyGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                SubjectId = g.SubjectId,
                SubjectName = g.Subject?.Name ?? string.Empty,
                AcademicYearId = g.AcademicYearId,
                AcademicYearName = g.AcademicYear?.Name ?? string.Empty,
                ProfessorId = g.ProfessorId,
                ProfessorName = $"{g.Professor?.User?.FirstName} {g.Professor?.User?.LastName}".Trim(),
                StudentsCount = g.Students.Count,
                AttendanceRate = 0,
                IsActive = g.IsActive
            });
        }
    }
}