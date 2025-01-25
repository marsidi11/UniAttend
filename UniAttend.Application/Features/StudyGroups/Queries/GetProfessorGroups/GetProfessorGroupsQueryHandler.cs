using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetProfessorStudyGroups
{
    public class GetProfessorStudyGroupsQueryHandler 
        : IRequestHandler<GetProfessorStudyGroupsQuery, IEnumerable<StudyGroupDto>>
    {
        private readonly IStudyGroupRepository _studyGroupRepository;

        public GetProfessorStudyGroupsQueryHandler(IStudyGroupRepository studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;
        }

        public async Task<IEnumerable<StudyGroupDto>> Handle(
            GetProfessorStudyGroupsQuery request, 
            CancellationToken cancellationToken)
        {
            var studyGroups = await _studyGroupRepository.GetByProfessorIdAsync(
                request.ProfessorId,
                request.AcademicYearId,
                cancellationToken);

            return studyGroups.Select(g => new StudyGroupDto
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