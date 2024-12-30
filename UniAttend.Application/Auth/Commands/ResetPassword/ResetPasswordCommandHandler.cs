using MediatR;
using UniAttend.Application.Auth.Common;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace UniAttend.Application.Auth.Commands.ResetPassword
{
    /// <summary>
    /// Handles the logic for resetting a user's password.
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException("User not found");
            }

            // Update the user's password
            var newHashedPassword = _passwordHasher.HashPassword(request.NewPassword);
            user.UpdatePassword(newHashedPassword);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Optionally, you can generate new tokens if needed
            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user);
            user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResult(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddHours(1),
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