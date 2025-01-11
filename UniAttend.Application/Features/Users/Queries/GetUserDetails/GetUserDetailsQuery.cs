using MediatR;
using UniAttend.Application.Features.Users.DTOs;

namespace UniAttend.Application.Features.Users.Queries.GetUserDetails
{
    public record GetUserDetailsQuery : IRequest<UserDetailsDto>
    {
        public int UserId { get; init; }
    }
}