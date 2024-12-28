using MediatR;
using UniAttend.Application.Auth.Common;

namespace UniAttend.Application.Features.Users.Queries.GetUserDetails
{
    public record GetUserDetailsQuery : IRequest<UserProfileDto>
    {
        public int UserId { get; init; }
    }
}