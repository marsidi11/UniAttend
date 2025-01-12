using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Enums;
using UniAttend.Shared.Exceptions;
using UniAttend.Shared.Utils;

namespace UniAttend.Application.Features.Students.Commands.RegisterStudent
{
    public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;

        public RegisterStudentCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<int> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
        {
            // Validate unique constraints
            if (await _unitOfWork.Students.StudentIdExistsAsync(request.StudentId, cancellationToken))
                throw new ValidationException("Student ID already exists");

            if (!string.IsNullOrEmpty(request.CardId) &&
                await _unitOfWork.Students.CardIdExistsAsync(request.CardId, cancellationToken))
                throw new ValidationException("Card ID already exists");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Generate secure random password
                string temporaryPassword = PasswordGenerator.GenerateTemporaryPassword();
                string hashedPassword = _passwordHasher.HashPassword(temporaryPassword);

                // Create user account with hashed password
                var user = new User(
                    username: request.StudentId,
                    passwordHash: hashedPassword,
                    email: request.Email,
                    role: UserRole.Student,
                    firstName: request.FirstName,
                    lastName: request.LastName
                );

                await _unitOfWork.Users.AddAsync(user, cancellationToken);

                // Create student record
                var student = new Student(request.StudentId, request.DepartmentId, user);
                if (!string.IsNullOrEmpty(request.CardId))
                {
                    student.AssignCard(request.CardId);
                }

                await _unitOfWork.Students.AddAsync(student, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                // Send welcome email with temporary password
                await _emailService.SendWelcomeEmailAsync(
                    request.Email,
                    $"{request.FirstName} {request.LastName}",
                    request.StudentId,
                    temporaryPassword, // Send original unhashed password
                    cancellationToken
                );

                return student.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}