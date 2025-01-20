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
            try
            {
                var professors = request.DepartmentId.HasValue 
                    ? await _professorRepository.GetByDepartmentId(request.DepartmentId.Value, cancellationToken)
                    : await _professorRepository.GetAllAsync(cancellationToken);
    
                return professors
                    .Where(p => !request.IsActive.HasValue || p.User?.IsActive == request.IsActive.Value)
                    .Select(p => new ProfessorDto
                    {
                        Id = p.Id,
                        UserId = p.Id,  // In 1:1 relationship, IDs are the same
                        FullName = $"{p.User?.FirstName} {p.User?.LastName}".Trim(),
                        Email = p.User?.Email ?? string.Empty,
                        DepartmentId = p.DepartmentId,
                        DepartmentName = p.Department?.Name ?? string.Empty,
                        IsActive = p.User?.IsActive ?? false
                    });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving professors", ex);
            }
        }
    }
}