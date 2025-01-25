using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.StudyGroups.Commands.UpdateStudyGroup
{
    public class UpdateStudyGroupCommandHandler : IRequestHandler<UpdateStudyGroupCommand, Unit>
    {
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudyGroupCommandHandler(IStudyGroupRepository studyGroupRepository, IUnitOfWork unitOfWork)
        {
            _studyGroupRepository = studyGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateStudyGroupCommand request, CancellationToken cancellationToken)
        {
            var studyGroup = await _studyGroupRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Study group not found");

            studyGroup.Update(request.Name, request.SubjectId, request.ProfessorId);
            
            if (studyGroup.IsActive != request.IsActive)
            {
                if (request.IsActive)
                    studyGroup.Activate();
                else
                    studyGroup.Deactivate();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}