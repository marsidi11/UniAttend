using MediatR;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Classes.Commands.CloseClass
{
    public class CloseClassCommandHandler : IRequestHandler<CloseClassCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CloseClassCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CloseClassCommand request, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.CourseSessions.GetByIdAsync(request.ClassId, cancellationToken)
                ?? throw new NotFoundException($"Class session with ID {request.ClassId} not found");

            session.UpdateStatus("Completed");
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}