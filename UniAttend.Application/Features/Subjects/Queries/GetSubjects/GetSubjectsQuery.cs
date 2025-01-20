using MediatR;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.Application.Features.Subjects.Queries.GetSubjects
{
    public record GetSubjectsQuery : IRequest<IEnumerable<SubjectDto>>
    {
        public int? DepartmentId { get; init; }
        public bool? IsActive { get; init; }
    }
}