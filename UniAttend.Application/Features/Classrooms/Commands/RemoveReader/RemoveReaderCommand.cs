using MediatR;

namespace UniAttend.Application.Features.Classrooms.Commands.RemoveReader
{
    public record RemoveReaderCommand : IRequest<Unit>
    {
        public int ClassroomId { get; init; }
    }
}