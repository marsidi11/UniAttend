using MediatR;

namespace UniAttend.Application.Features.StudyGroups.Commands.TransferStudent
{
    public record TransferStudentCommand : IRequest<Unit>
    {
        public int StudentId { get; init; }
        public int FromGroupId { get; init; }
        public int ToGroupId { get; init; }
    }
}