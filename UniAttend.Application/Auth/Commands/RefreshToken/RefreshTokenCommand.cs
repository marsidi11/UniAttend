using MediatR;
using UniAttend.Application.Auth.Common;

namespace UniAttend.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<AuthResult>
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}