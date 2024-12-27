using MediatR;
using UniAttend.Application.Auth.Common;

/// <summary>
/// Represents a command to authenticate a user with their credentials.
/// </summary>
/// <remarks>
/// This command is used as part of the authentication process to validate user credentials
/// and generate an authentication result.
/// </remarks>
/// <seealso cref="IRequest{AuthResult}"/>
namespace UniAttend.Application.Auth.Commands.Login
{
    public record LoginCommand : IRequest<AuthResult>
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}