using MediatR;

namespace UniAttend.Application.Features.Classrooms.Commands.UpdateClassroom
{
    public record UpdateClassroomCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? ReaderDeviceId { get; init; }
    }
}