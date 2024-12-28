using MediatR;
using UniAttend.Application.Common.Interfaces;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthService _authService;

        public ChangePasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found");

            // Verify current password
            if (!await _authService.ValidateCredentialsAsync(user.Username, request.CurrentPassword))
                throw new ValidationException("Current password is incorrect");

            // Update password
            var newHashedPassword = _passwordHasher.HashPassword(request.NewPassword);
            user.UpdatePassword(newHashedPassword);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}