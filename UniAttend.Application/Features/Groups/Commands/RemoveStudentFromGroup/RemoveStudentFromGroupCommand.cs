using MediatR;

namespace UniAttend.Application.Features.Groups.Commands.RemoveStudentFromGroup
{
    public record RemoveStudentFromGroupCommand : IRequest<Unit>
    {
        public int GroupId { get; init; }
        public int StudentId { get; init; }
    }
}