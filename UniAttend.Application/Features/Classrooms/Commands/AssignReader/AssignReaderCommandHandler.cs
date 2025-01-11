using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Classrooms.Commands.AssignReader
{
    public class AssignReaderCommandHandler : IRequestHandler<AssignReaderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignReaderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AssignReaderCommand request, CancellationToken cancellationToken)
        {
            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(request.ClassroomId, cancellationToken)
                ?? throw new NotFoundException($"Classroom with ID {request.ClassroomId} not found");

            classroom.AssignReader(request.ReaderDeviceId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}