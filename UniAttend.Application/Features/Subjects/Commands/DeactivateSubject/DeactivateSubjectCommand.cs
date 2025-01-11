using MediatR;

namespace UniAttend.Application.Features.Subjects.Commands.DeactivateSubject
{
    public record DeactivateSubjectCommand : IRequest<Unit>
    {
        public int Id { get; init; }
    }
}