using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Professors.DTOs;

namespace UniAttend.Application.Features.Professors.Queries.GetProfessors
{
    public class GetProfessorsQueryHandler 
        : IRequestHandler<GetProfessorsQuery, IEnumerable<ProfessorDto>>
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IMapper _mapper;

        public GetProfessorsQueryHandler(IProfessorRepository professorRepository, IMapper mapper)
        {
            _professorRepository = professorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProfessorDto>> Handle(
            GetProfessorsQuery request, 
            CancellationToken cancellationToken)
        {
            var professors = await _professorRepository.GetByDepartmentId(
                request.DepartmentId ?? 0, 
                cancellationToken);

            return professors
                .Where(p => !request.IsActive.HasValue || p.User.IsActive == request.IsActive.Value)
                .Select(p => new ProfessorDto
                {
                    Id = p.Id,
                    UserId = p.Id,
                    FullName = $"{p.User?.FirstName} {p.User?.LastName}".Trim(),
                    Email = p.User?.Email ?? string.Empty,
                    DepartmentId = p.DepartmentId,
                    DepartmentName = p.Department?.Name ?? string.Empty,
                    IsActive = p.User?.IsActive ?? false
                });
        }
    }
}