using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Entities;

namespace UniAttend.Application.Features.Subjects.Commands.CreateSubject
{
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.DepartmentId} not found");

            var subject = new Subject(
                name: request.Name,
                description: request.Description,
                credits: request.Credits,
                department: department
            );

            await _unitOfWork.Subjects.AddAsync(subject, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return subject.Id;
        }
    }
}