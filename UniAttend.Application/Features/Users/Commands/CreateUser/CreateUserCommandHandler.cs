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
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Role != UserRole.Secretary && request.Role != UserRole.Professor)
                throw new ValidationException("Invalid role for User member");

            if (await _unitOfWork.Users.UsernameExistsAsync(request.Username, cancellationToken))
                throw new ValidationException("Username already exists");

            if (await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
                throw new ValidationException("Email already exists");

            // Skip department validation for Secretary role
            Department? department = null;
            if (request.Role == UserRole.Professor)
            {
                if (!request.DepartmentId.HasValue)
                    throw new ValidationException("Department is required for professors");
                    
                department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId.Value, cancellationToken)
                    ?? throw new NotFoundException($"Department with ID {request.DepartmentId.Value} not found");
            }
        
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
                await _unitOfWork.Users.AddAsync(user, cancellationToken);
        
                // Only create Professor entity if role is Professor
                if (request.Role == UserRole.Professor && department != null)
                {
                    var professor = new Professor(department.Id, user);
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
    }
}