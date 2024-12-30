using MediatR;

namespace UniAttend.Application.Features.Subjects.Commands.CreateSubject
{
    public record CreateSubjectCommand : IRequest<int>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int DepartmentId { get; init; }
        public int Credits { get; init; }
    }
}