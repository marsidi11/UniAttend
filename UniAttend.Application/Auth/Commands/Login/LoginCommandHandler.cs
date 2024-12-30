using MediatR;
using UniAttend.Application.Auth.Common;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;

/// <summary>
/// Handles the authentication process for user login requests.
/// This handler validates user credentials, generates authentication tokens,
/// and updates the user's refresh token information.
/// </summary>
/// <remarks>
/// The handler performs the following operations:
/// 1. Validates user credentials against the stored user information
/// 2. Generates new access and refresh tokens
/// 3. Updates the user's refresh token and login timestamp
/// 4. Returns authentication result with user information and tokens
/// </remarks>
namespace UniAttend.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResult> Handle(
            LoginCommand request, 
            CancellationToken cancellationToken)
        {
            // Validate credentials
            var user = await _userRepository.GetByUsernameAsync(
                request.Username, cancellationToken);

            if (user == null || !await _authService.ValidateCredentialsAsync(
                request.Username, request.Password))
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            // Generate tokens
            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user);

            // Update user's refresh token
            user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            user.RecordLogin();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return authentication result
            return new AuthResult(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddHours(1), // Token expiry
                new UserDto(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Role));
        }
    }
}