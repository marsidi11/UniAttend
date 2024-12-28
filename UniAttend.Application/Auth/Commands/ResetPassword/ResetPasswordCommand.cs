using MediatR;
using UniAttend.Application.Auth.Common;

namespace UniAttend.Application.Auth.Commands.ResetPassword
{
    /// <summary>
    /// Represents a command to reset a user's password.
    /// </summary>
    public record ResetPasswordCommand : IRequest<AuthResult>
    {
        /// <summary>
        /// The user's unique identifier.
        /// </summary>
        public int UserId { get; init; }

        /// <summary>
        /// The new password for the user.
        /// </summary>
        public string NewPassword { get; init; } = string.Empty;
    }
}