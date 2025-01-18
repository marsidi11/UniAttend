using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Utils;

namespace UniAttend.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
                
                // For security, don't reveal if email exists or not
                if (user == null || !user.IsActive)
                {
                    return Unit.Value;
                }

                // Generate new temporary password
                string newPassword = PasswordGenerator.GenerateTemporaryPassword();
                string hashedPassword = _passwordHasher.HashPassword(newPassword);

                // Update user's password
                user.UpdatePassword(hashedPassword);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Send password reset email
                await _emailService.SendPasswordResetEmailAsync(
                    user.Email,
                    $"{user.FirstName} {user.LastName}",
                    user.Username,
                    newPassword,
                    cancellationToken
                );

                await _unitOfWork.CommitAsync(cancellationToken);
                return Unit.Value;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}