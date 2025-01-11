using MediatR;
using UniAttend.Application.Auth.Common;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
            
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid or expired refresh token");
            }

            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user);
            
            user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResult(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddHours(1),
                new UserAuthDto(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Role));
        }
    }
}