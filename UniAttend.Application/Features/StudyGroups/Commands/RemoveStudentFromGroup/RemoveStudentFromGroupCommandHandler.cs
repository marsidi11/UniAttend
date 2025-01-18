using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Exceptions;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.StudyGroups.Commands.RemoveStudentFromGroup
{
    public class RemoveStudentFromGroupCommandHandler : IRequestHandler<RemoveStudentFromGroupCommand, Unit>
    {
        private readonly IStudyGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveStudentFromGroupCommandHandler(
            IStudyGroupRepository groupRepository,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveStudentFromGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken)
                ?? throw new NotFoundException("Study group not found");

            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException("Student not found");

            group.RemoveStudent(student.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}