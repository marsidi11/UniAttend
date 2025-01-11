using MediatR;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public int? DepartmentId { get; init; }
        public bool IsActive { get; init; }
    }
}