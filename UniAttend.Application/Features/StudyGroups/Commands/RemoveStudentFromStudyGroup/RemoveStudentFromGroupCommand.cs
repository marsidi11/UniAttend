using MediatR;

namespace UniAttend.Application.Features.StudyGroups.Commands.RemoveStudentFromStudyGroup
{
    public record RemoveStudentFromStudyGroupCommand : IRequest<Unit>
    {
        public int StudyGroupId { get; init; }
        public int StudentId { get; init; }
    }
}