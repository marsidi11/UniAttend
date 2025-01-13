using MediatR;

namespace UniAttend.Application.Features.Auth.Commands.Logout
{
    /// <summary>
    /// Represents a command to log out a user.
    /// </summary>
    public record LogoutCommand : IRequest<Unit>
    {
        /// <summary>
        /// The user's unique identifier.
        /// </summary>
        public int UserId { get; init; }
    }
}