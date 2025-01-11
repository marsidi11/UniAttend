using MediatR;

namespace UniAttend.Application.Features.Classrooms.Commands.CreateClassroom
{
    public record CreateClassroomCommand : IRequest<int>
    {
        public string Name { get; init; } = string.Empty;
        public string? ReaderDeviceId { get; init; }
    }
}