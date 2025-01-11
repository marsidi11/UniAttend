using MediatR;

namespace UniAttend.Application.Features.Users.Commands.DeactivateUser
{
    public record DeactivateUserCommand : IRequest<Unit>
    {
        public int Id { get; init; }
    }
}