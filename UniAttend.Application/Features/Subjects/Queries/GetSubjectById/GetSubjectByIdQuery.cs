using MediatR;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.Application.Features.Subjects.Queries.GetSubjectById
{
    public record GetSubjectByIdQuery : IRequest<SubjectDto?>
    {
        public int Id { get; init; }
    }
}