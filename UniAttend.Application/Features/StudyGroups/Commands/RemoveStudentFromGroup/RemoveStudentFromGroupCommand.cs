using MediatR;

namespace UniAttend.Application.Features.StudyGroups.Commands.RemoveStudentFromGroup
{
    public record RemoveStudentFromGroupCommand : IRequest<Unit>
    {
        public int StudyGroupId { get; init; }
        public int StudentId { get; init; }
    }
}