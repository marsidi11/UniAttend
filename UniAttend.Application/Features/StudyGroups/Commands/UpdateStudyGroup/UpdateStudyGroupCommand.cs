using MediatR;

namespace UniAttend.Application.Features.StudyGroups.Commands.UpdateStudyGroup
{
    public record UpdateStudyGroupCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int SubjectId { get; init; }
        public int ProfessorId { get; init; }
        public bool IsActive { get; init; }
    }
}