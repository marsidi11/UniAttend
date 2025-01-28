using MediatR;
using UniAttend.Core.Enums;
using UniAttend.Application.Features.Users.DTOs;

namespace UniAttend.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public int? Id { get; init; }
        public UserRole? Role { get; init; }
        public bool? IsActive { get; init; }
    }
}