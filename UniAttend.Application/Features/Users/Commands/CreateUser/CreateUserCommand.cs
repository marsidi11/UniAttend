using MediatR;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<int>
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public UserRole Role { get; init; }
        public int DepartmentId { get; init; }
    }
}