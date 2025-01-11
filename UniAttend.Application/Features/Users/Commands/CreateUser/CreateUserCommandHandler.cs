using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Shared.Exceptions;

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
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Role != UserRole.Secretary && request.Role != UserRole.Professor)
                throw new ValidationException("Invalid role for User member");

            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.DepartmentId} not found");

            var tempPassword = GenerateTemporaryPassword();

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
                await _unitOfWork.Users.AddAsync(user, cancellationToken);

                if (request.Role == UserRole.Professor)
                {
                    var professor = new Professor(request.DepartmentId, user);
                    await _unitOfWork.Professors.AddAsync(professor, cancellationToken);
                }

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

        private static string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}