using MediatR;
using UniAttend.Application.Auth.Common; 

namespace UniAttend.Application.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<AuthResult>
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Role { get; init; }
        // Student specific fields
        public string? StudentId { get; init; }
        public int? DepartmentId { get; init; }
        public string? CardId { get; init; }
    }
}