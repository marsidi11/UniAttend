using MediatR;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.StudyGroups.Commands.EnrollStudents
{
    public class EnrollStudentsCommandHandler : IRequestHandler<EnrollStudentsCommand, Unit>
    {
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollStudentsCommandHandler(
        IGroupStudentRepository groupStudentRepository,
        IUnitOfWork unitOfWork)
        {
            _groupStudentRepository = groupStudentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(EnrollStudentsCommand request, CancellationToken cancellationToken)
        {
            foreach (var studentId in request.StudentIds)
            {
                if (!await _groupStudentRepository.ExistsAsync(request.GroupId, studentId, cancellationToken))
                {
                    await _groupStudentRepository.AddStudentToGroupAsync(request.GroupId, studentId, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}