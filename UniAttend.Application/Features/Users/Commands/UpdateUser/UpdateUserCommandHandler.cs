using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("User member not found");

            if (user.Email != request.Email &&
                await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
                throw new ValidationException("Email already in use");

            user.UpdateProfile(request.FirstName, request.LastName, request.Email);
            user.UpdateActiveStatus(request.IsActive); // Use new method

            if (user.Role == UserRole.Professor && request.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(
                    request.DepartmentId.Value,
                    cancellationToken)
                    ?? throw new NotFoundException($"Department with ID {request.DepartmentId.Value} not found");

                var professor = await _unitOfWork.Professors.GetByUserIdAsync(user.Id, cancellationToken)
                    ?? throw new NotFoundException($"Professor record not found for user {user.Id}");

                professor.AssignDepartment(department);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}