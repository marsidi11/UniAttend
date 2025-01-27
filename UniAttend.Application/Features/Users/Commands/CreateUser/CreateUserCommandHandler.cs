using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Shared.Exceptions;
using UniAttend.Shared.Utils;

namespace UniAttend.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
    
        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            
            // Validation
            if (request.Role != UserRole.Secretary && request.Role != UserRole.Professor)
                throw new ValidationException("Invalid role for User member");

            if (await _unitOfWork.Users.UsernameExistsAsync(request.Username, cancellationToken))
                throw new ValidationException("Username already exists");

            if (await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
                throw new ValidationException("Email already exists");

            var tempPassword = PasswordGenerator.GenerateTemporaryPassword();
            var user = new User(
                request.Username,
                _passwordHasher.HashPassword(tempPassword),
                request.Email,
                request.Role,
                request.FirstName,
                request.LastName
            );

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Create user with role and departments
                await _unitOfWork.Users.CreateUserWithRoleAsync(
                    user,
                    request.Role,
                    request.DepartmentIds,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                await _emailService.SendWelcomeEmailAsync(
                    user.Email,
                    $"{user.FirstName} {user.LastName}",
                    request.Username,
                    tempPassword,
                    cancellationToken
                );

                return user.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}