using MediatR;
using UniAttend.Core.Interfaces;
using UniAttend.Application.Auth.Common;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniAttend.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResult> Handle(
            RegisterCommand request, 
            CancellationToken cancellationToken)
        {
            // Validate unique constraints
            if (await _unitOfWork.Users.UsernameExistsAsync(request.Username, cancellationToken))
                throw new ValidationException("Username already exists");

            if (await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
                throw new ValidationException("Email already exists");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var userRole = Enum.Parse<UserRole>(request.Role);
                
                // Create user
                var user = new User(
                    request.Username,
                    _passwordHasher.HashPassword(request.Password),
                    request.Email,
                    userRole,
                    request.FirstName,
                    request.LastName
                );

                await _unitOfWork.Users.AddAsync(user, cancellationToken);

                // Handle student registration
                if (userRole == UserRole.Student && request.StudentId != null && request.DepartmentId != null)
                {
                    var student = new Student(request.StudentId!, request.DepartmentId!.Value, user);
                    if (!string.IsNullOrEmpty(request.CardId))
                    {
                        student.AssignCard(request.CardId);
                    }
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                // Generate tokens
                var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user);

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
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}