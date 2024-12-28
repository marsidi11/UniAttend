using MediatR;

namespace UniAttend.Application.Features.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
    }
}