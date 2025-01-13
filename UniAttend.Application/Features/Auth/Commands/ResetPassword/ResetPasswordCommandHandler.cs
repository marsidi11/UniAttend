using MediatR;
using UniAttend.Application.Features.Auth.DTOs;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace UniAttend.Application.Features.Auth.Commands.ResetPassword
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
        private readonly IEmailService _emailService;
    
        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }
    
        public async Task<AuthResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException("User not found");
            }
    
            // Generate new password
            string newPassword = PasswordGenerator.GenerateTemporaryPassword();
            var newHashedPassword = _passwordHasher.HashPassword(newPassword);
            
            // Update password
            user.UpdatePassword(newHashedPassword);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
    
            // Send email with new password
            await _emailService.SendPasswordResetEmailAsync(
                user.Email,
                $"{user.FirstName} {user.LastName}",
                user.Username,
                newPassword,
                cancellationToken
            );
    
            // Generate new tokens
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