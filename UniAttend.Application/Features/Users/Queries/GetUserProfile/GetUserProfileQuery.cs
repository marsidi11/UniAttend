using MediatR;
using UniAttend.Application.Features.Users.DTOs;

namespace UniAttend.Application.Features.Users.Queries.GetUserProfile
{
    public record GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public int UserId { get; init; }
    }
}