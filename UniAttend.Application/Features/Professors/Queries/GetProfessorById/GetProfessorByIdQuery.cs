using MediatR;
using UniAttend.Application.Features.Professors.DTOs;

namespace UniAttend.Application.Features.Professors.Queries.GetProfessorById
{
    public record GetProfessorByIdQuery : IRequest<ProfessorDto?>
    {
        public int Id { get; init; }
    }
}