using MediatR;
using UniAttend.Application.Features.Auth.DTOs;

namespace UniAttend.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<AuthResult>
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string? StudentId { get; init; }
        public int? DepartmentId { get; init; }
        public string? CardId { get; init; }
    }
}