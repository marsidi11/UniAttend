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
            // Validate unique constraints first
            if (await _unitOfWork.Students.StudentIdExistsAsync(request.StudentId, cancellationToken))
                throw new ValidationException("Student ID already exists");

            if (!string.IsNullOrEmpty(request.CardId) &&
                await _unitOfWork.Students.CardIdExistsAsync(request.CardId, cancellationToken))
                throw new ValidationException("Card ID already exists");

            if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
                throw new ValidationException("Email {request.Email} is already registered");

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                // Generate username and password
                string username = await GenerateUniqueUsernameAsync(request.FirstName, request.LastName, cancellationToken);
                string temporaryPassword = PasswordGenerator.GenerateTemporaryPassword();
                string hashedPassword = _passwordHasher.HashPassword(temporaryPassword);

                // Create user first
                var user = new User(
                    username: username,
                    passwordHash: hashedPassword,
                    email: request.Email,
                    role: UserRole.Student,
                    firstName: request.FirstName,
                    lastName: request.LastName
                );

                await _unitOfWork.Users.AddAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Then create student
                var student = new Student(request.StudentId, request.DepartmentId, user);
                if (!string.IsNullOrEmpty(request.CardId))
                {
                    student.AssignCard(request.CardId);
                }

                await _unitOfWork.Students.AddAsync(student, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                // Send welcome email after successful commit
                await _emailService.SendWelcomeEmailAsync(
                    request.Email,
                    $"{request.FirstName} {request.LastName}",
                    username,
                    temporaryPassword,
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

        private async Task<string> GenerateUniqueUsernameAsync(string firstName, string lastName, CancellationToken cancellationToken)
        {
            // Convert to lowercase and remove spaces/special characters
            firstName = firstName.ToLower().Trim();
            lastName = lastName.ToLower().Trim();

            // Take first letter of first name + full lastname
            string baseUsername = $"{firstName[0]}.{lastName}";

            // Remove any special characters and spaces
            baseUsername = string.Join("", baseUsername.Where(c => c == '.' || char.IsLetterOrDigit(c)));

            string username = baseUsername;
            int counter = 1;

            // Check if username exists and append number if it does
            while (await _unitOfWork.Users.UsernameExistsAsync(username, cancellationToken))
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            return username;
        }
    }
}