using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Classrooms.Commands.UpdateClassroom
{
    public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClassroomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateClassroomCommand request, CancellationToken cancellationToken)
        {
            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Classroom with ID {request.Id} not found");

            if (request.Name != classroom.Name && 
                await _unitOfWork.Classrooms.ExistsWithNameAsync(request.Name, cancellationToken))
                throw new ValidationException($"Classroom with name {request.Name} already exists");

            classroom = new Classroom(request.Name, request.ReaderDeviceId);
            
            await _unitOfWork.Classrooms.UpdateAsync(classroom);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}