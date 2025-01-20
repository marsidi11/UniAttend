using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroupById
{
    public class GetStudyGroupByIdQueryHandler : IRequestHandler<GetStudyGroupByIdQuery, StudyGroupDto?>
    {
        private readonly IStudyGroupRepository _groupRepository;

        public GetStudyGroupByIdQueryHandler(IStudyGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<StudyGroupDto?> Handle(GetStudyGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            
            if (group == null) return null;

            return new StudyGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                SubjectId = group.SubjectId,
                SubjectName = group.Subject?.Name ?? string.Empty,
                AcademicYearId = group.AcademicYearId,
                AcademicYearName = group.AcademicYear?.Name ?? string.Empty,
                ProfessorId = group.ProfessorId,
                ProfessorName = $"{group.Professor?.User?.FirstName} {group.Professor?.User?.LastName}".Trim(),
                StudentsCount = group.Students.Count,
                AttendanceRate = 0,
                IsActive = group.IsActive
            };
        }
    }
}