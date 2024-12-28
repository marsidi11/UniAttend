using MediatR;
using UniAttend.Application.Auth.Common;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Common.Exceptions;

namespace UniAttend.Application.Auth.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetUserProfileQueryHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedException("User is not authenticated");
            }

            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId.Value, cancellationToken);
            
            if (user == null)
            {
                throw new NotFoundException("User profile not found");
            }

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