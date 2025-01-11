using MediatR;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Classrooms.Commands.CreateClassroom
{
    public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateClassroomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Classrooms.ExistsWithNameAsync(request.Name, cancellationToken))
                throw new ValidationException($"Classroom with name {request.Name} already exists");

            var classroom = new Classroom(request.Name, request.ReaderDeviceId);
            
            await _unitOfWork.Classrooms.AddAsync(classroom, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return classroom.Id;
        }
    }
}