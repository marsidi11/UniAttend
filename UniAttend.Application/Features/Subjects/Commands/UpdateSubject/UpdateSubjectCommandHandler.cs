using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Subjects.Commands.UpdateSubject
{
    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSubjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

                public async Task<Unit> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Subject with ID {request.Id} not found");
        
            if (await _unitOfWork.Subjects.ExistsInDepartmentAsync(
                request.Name,
                subject.DepartmentId,
                cancellationToken) &&
                subject.Name != request.Name)
            {
                throw new ValidationException("A subject with this name already exists in the department");
            }
        
            subject.Update(request.Name, request.Description, request.Credits);
        
            if (subject.IsActive != request.IsActive)
            {
                subject.SetActive(request.IsActive);
            }
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}