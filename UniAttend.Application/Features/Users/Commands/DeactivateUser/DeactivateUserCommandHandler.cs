using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Enums;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditService _auditService;

        public DeactivateUserCommandHandler(IUnitOfWork unitOfWork, IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"User with ID {request.Id} not found");

            if (!user.IsActive)
                throw new ValidationException("User is already deactivated");

            if (user.Role == UserRole.Admin)
                throw new ValidationException("Admin users cannot be deactivated");

            user.UpdateActiveStatus(false);
            
            await _auditService.LogActionAsync(
                "DEACTIVATE_USER",
                "User",
                user.Id,
                request.Id,
                $"Deactivated user {user.Username}",
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}