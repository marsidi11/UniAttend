using MediatR;
using UniAttend.Application.Auth.Common;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            
            if (user == null)
                throw new NotFoundException("User profile not found");

            return new UserProfileDto(
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role,
                user.LastLoginDate);
        }
    }
}