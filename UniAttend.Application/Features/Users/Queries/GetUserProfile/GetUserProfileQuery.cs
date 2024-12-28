using MediatR;
using UniAttend.Application.Auth.Common;

namespace UniAttend.Application.Features.Users.Queries.GetUserProfile
{
    public record GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public int UserId { get; init; }
    }
}