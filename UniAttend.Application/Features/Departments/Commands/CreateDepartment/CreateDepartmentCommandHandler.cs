using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Departments.Commands.CreateDepartment;
using UniAttend.Core.Entities;
using MediatR;

namespace UniAttend.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = new Department(request.Name);
            await _unitOfWork.Departments.AddAsync(department, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return department.Id;
        }
    }
}