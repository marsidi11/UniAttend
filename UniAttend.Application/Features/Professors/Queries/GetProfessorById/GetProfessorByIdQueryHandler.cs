using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Professors.DTOs;
using UniAttend.Application.Features.Departments.DTOs;

namespace UniAttend.Application.Features.Professors.Queries.GetProfessorById
{
    public class GetProfessorByIdQueryHandler
        : IRequestHandler<GetProfessorByIdQuery, ProfessorDto?>
    {
        private readonly IProfessorRepository _professorRepository;

        public GetProfessorByIdQueryHandler(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public async Task<ProfessorDto?> Handle(GetProfessorByIdQuery request, CancellationToken cancellationToken)
        {
            var professor = await _professorRepository.GetByIdAsync(request.Id, cancellationToken);

            if (professor == null) return null;

            return new ProfessorDto
            {
                Id = professor.Id,
                UserId = professor.Id,
                FullName = $"{professor.User?.FirstName} {professor.User?.LastName}".Trim(),
                Email = professor.User?.Email ?? string.Empty,
                Departments = professor.Departments.Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }),
                IsActive = professor.User?.IsActive ?? false
            };
        }
    }
}