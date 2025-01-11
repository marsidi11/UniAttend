using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Classrooms.Commands.RemoveReader
{
    public class RemoveReaderCommandHandler : IRequestHandler<RemoveReaderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveReaderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveReaderCommand request, CancellationToken cancellationToken)
        {
            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(request.ClassroomId, cancellationToken)
                ?? throw new NotFoundException($"Classroom with ID {request.ClassroomId} not found");

            classroom.RemoveReader();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}