using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroupById
{
    public class GetStudyGroupByIdQueryHandler : IRequestHandler<GetStudyGroupByIdQuery, StudyGroupDto?>
    {
        private readonly IStudyGroupRepository _studyGroupRepository;

        public GetStudyGroupByIdQueryHandler(IStudyGroupRepository studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;
        }

        public async Task<StudyGroupDto?> Handle(GetStudyGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var studyGroup = await _studyGroupRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            
            if (studyGroup == null) return null;

            return new StudyGroupDto
            {
                Id = studyGroup.Id,
                Name = studyGroup.Name,
                SubjectId = studyGroup.SubjectId,
                SubjectName = studyGroup.Subject?.Name ?? string.Empty,
                AcademicYearId = studyGroup.AcademicYearId,
                AcademicYearName = studyGroup.AcademicYear?.Name ?? string.Empty,
                ProfessorId = studyGroup.ProfessorId,
                ProfessorName = $"{studyGroup.Professor?.User?.FirstName} {studyGroup.Professor?.User?.LastName}".Trim(),
                StudentsCount = studyGroup.Students.Count,
                AttendanceRate = 0,
                IsActive = studyGroup.IsActive
            };
        }
    }
}