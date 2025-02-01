using MediatR;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.CourseSessions.Commands.CloseCourseSession
{
    /// <summary>
    /// Handles the command to close a course session.
    /// </summary>
    public class CloseCourseSessionCommandHandler : IRequestHandler<CloseCourseSessionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseCourseSessionCommandHandler"/> class.
        /// </summary>
        public CloseCourseSessionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processes the command to close a course session.
        /// </summary>
        public async Task<Unit> Handle(CloseCourseSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.CourseSessions.GetByIdAsync(request.CourseSessionId, cancellationToken)
                ?? throw new NotFoundException($"Class session with ID {request.CourseSessionId} not found");

            session.UpdateStatus(SessionStatus.Completed);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}