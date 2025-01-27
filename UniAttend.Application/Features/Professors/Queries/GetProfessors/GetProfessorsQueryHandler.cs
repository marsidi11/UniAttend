using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Professors.DTOs;
using UniAttend.Application.Features.Departments.DTOs;
using Microsoft.Extensions.Logging;

namespace UniAttend.Application.Features.Professors.Queries.GetProfessors
{
        public class GetProfessorsQueryHandler 
        : IRequestHandler<GetProfessorsQuery, IEnumerable<ProfessorDto>>
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProfessorsQueryHandler> _logger;
    
        public GetProfessorsQueryHandler(
            IProfessorRepository professorRepository, 
            IMapper mapper,
            ILogger<GetProfessorsQueryHandler> logger) 
        {
            _professorRepository = professorRepository;
            _mapper = mapper;
            _logger = logger; 
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
        
                _logger.LogInformation($"Retrieved {professors.Count()} professors from database");
                
                var filtered = professors
                    .Where(p => !request.IsActive.HasValue || p.User?.IsActive == request.IsActive.Value);
        
                _logger.LogInformation($"After filtering: {filtered.Count()} professors");
        
                return filtered.Select(p => new ProfessorDto
                {
                    Id = p.Id,
                    UserId = p.Id,
                    FullName = $"{p.User?.FirstName} {p.User?.LastName}".Trim(),
                    Email = p.User?.Email ?? string.Empty,
                    Departments = p.Departments.Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        IsActive = d.IsActive
                    }),
                    IsActive = p.User?.IsActive ?? false
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving professors"); 
                throw;
            }
        }
    }
}