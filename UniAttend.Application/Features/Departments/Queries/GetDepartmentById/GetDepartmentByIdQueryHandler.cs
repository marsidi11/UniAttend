using AutoMapper;
using MediatR;
using UniAttend.Application.Features.Departments.DTOs;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.Id} not found");

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            
            // Enrich with counts
            departmentDto = departmentDto with
            {
                SubjectsCount = department.Subjects.Count,
                StudentsCount = department.Students.Count,
                ProfessorsCount = department.Professors.Count
            };

            return departmentDto;
        }
    }
}