using MediatR;
using UniAttend.Application.Features.Groups.DTOs;

namespace UniAttend.Application.Features.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand : IRequest<StudyGroupDto>
    {
        public string Name { get; init; } = string.Empty;
        public int SubjectId { get; init; }
        public int AcademicYearId { get; init; }
        public int ProfessorId { get; init; }
    }
}