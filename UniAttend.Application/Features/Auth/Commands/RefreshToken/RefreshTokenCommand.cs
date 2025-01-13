using MediatR;
using UniAttend.Application.Features.Auth.DTOs;

namespace UniAttend.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<AuthResult>
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}