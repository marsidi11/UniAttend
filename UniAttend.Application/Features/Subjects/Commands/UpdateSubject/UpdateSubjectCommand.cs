using MediatR;

namespace UniAttend.Application.Features.Subjects.Commands.UpdateSubject
{
    public record UpdateSubjectCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int Credits { get; init; }
        public bool IsActive { get; init; }
    }
}