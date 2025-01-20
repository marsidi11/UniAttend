using MediatR;
using UniAttend.Application.Features.Professors.DTOs;

namespace UniAttend.Application.Features.Professors.Queries.GetProfessors
{
    public record GetProfessorsQuery : IRequest<IEnumerable<ProfessorDto>>
    {
        public int? DepartmentId { get; init; }
        public bool? IsActive { get; init; }
    }
}