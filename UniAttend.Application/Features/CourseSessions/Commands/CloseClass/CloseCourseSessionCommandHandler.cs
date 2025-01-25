using MediatR;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.CourseSessions.Commands.CloseCourseSession
{
    public class CloseCourseSessionCommandHandler : IRequestHandler<CloseCourseSessionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CloseCourseSessionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CloseCourseSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.CourseSessions.GetByIdAsync(request.CourseSessionId, cancellationToken)
                ?? throw new NotFoundException($"Class session with ID {request.CourseSessionId} not found");

            session.UpdateStatus("Completed");
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}