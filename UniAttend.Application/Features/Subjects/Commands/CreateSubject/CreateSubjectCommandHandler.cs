using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Entities;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.Application.Features.Subjects.Commands.CreateSubject
{
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, SubjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSubjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

                public async Task<SubjectDto> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            // Check if department exists
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.DepartmentId} not found");
        
            // Check if subject name already exists in department
            if (await _unitOfWork.Subjects.ExistsInDepartmentAsync(request.Name, request.DepartmentId, cancellationToken))
            {
                throw new ValidationException($"A subject with name '{request.Name}' already exists in this department");
            }
        
            var subject = new Subject(
                name: request.Name,
                description: request.Description,
                credits: request.Credits,
                department: department
            );
        
            await _unitOfWork.Subjects.AddAsync(subject, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            // Use the mapping profile directly
            return _mapper.Map<SubjectDto>(subject);
        }
    }
}