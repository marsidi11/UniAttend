using MediatR;
using UniAttend.Application.Auth.Common;

namespace UniAttend.Application.Auth.Queries.GetUserProfile
{
    public record GetUserProfileQuery : IRequest<UserProfileDto>;
}