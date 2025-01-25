using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Exceptions;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.StudyGroups.Commands.RemoveStudentFromStudyGroup
{
    public class RemoveStudentFromStudyGroupCommandHandler : IRequestHandler<RemoveStudentFromStudyGroupCommand, Unit>
    {
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveStudentFromStudyGroupCommandHandler(
            IStudyGroupRepository studyGroupRepository,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork)
        {
            _studyGroupRepository = studyGroupRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveStudentFromStudyGroupCommand request, CancellationToken cancellationToken)
        {
            var studyGroup = await _studyGroupRepository.GetByIdAsync(request.StudyGroupId, cancellationToken)
                ?? throw new NotFoundException("Study group not found");

            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException("Student not found");

            studyGroup.RemoveStudent(student.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}