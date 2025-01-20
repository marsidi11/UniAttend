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
            // First verify department exists
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.DepartmentId} not found");

            // Check if subject name already exists in department
            if (await _unitOfWork.Subjects.ExistsInDepartmentAsync(
                request.Name,
                request.DepartmentId,
                cancellationToken))
            {
                throw new ValidationException($"A subject with name '{request.Name}' already exists in this department");
            }

            // Create new subject
            var subject = new Subject(
                request.Name,
                request.Description,
                request.Credits,
                request.DepartmentId
            );

            await _unitOfWork.Subjects.AddAsync(subject, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var subjectDto = _mapper.Map<SubjectDto>(subject);
            return subjectDto with { DepartmentName = department.Name };
        }
    }
}