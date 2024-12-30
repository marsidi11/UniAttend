using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Services;
using MediatR;
using UniAttend.Application.Common.Exceptions;

namespace UniAttend.Application.Features.Students.Commands.RegisterStudent
{
    public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterStudentCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
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
                // Create user account
                var user = new User(
                    username: request.StudentId, // Use student ID as username
                    passwordHash: _passwordHasher.HashPassword(request.StudentId), // Initial password is student ID
                    email: request.Email,
                    role: UserRole.Student,
                    firstName: request.FirstName,
                    lastName: request.LastName
                );

                await _unitOfWork.Users.AddAsync(user, cancellationToken);

                // Create student record
                var student = new Student(request.StudentId, request.DepartmentId, user)
                {
                    CardId = request.CardId
                };

                await _unitOfWork.Students.AddAsync(student, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

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