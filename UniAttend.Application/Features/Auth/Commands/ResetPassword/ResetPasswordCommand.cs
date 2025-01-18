using MediatR;

namespace UniAttend.Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; init; } = string.Empty;
    }
}