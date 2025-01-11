using MediatR;

namespace UniAttend.Application.Features.Classrooms.Commands.AssignReader
{
    public record AssignReaderCommand : IRequest<Unit>
    {
        public int ClassroomId { get; init; }
        public string ReaderDeviceId { get; init; } = string.Empty;
    }
}