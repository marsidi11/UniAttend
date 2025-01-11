using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Subjects.Commands.DeactivateSubject
{
    public class DeactivateSubjectCommandHandler : IRequestHandler<DeactivateSubjectCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateSubjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeactivateSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Subject with ID {request.Id} not found");

            subject.SetDeactive();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}