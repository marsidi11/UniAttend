using MediatR;

namespace UniAttend.Application.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}