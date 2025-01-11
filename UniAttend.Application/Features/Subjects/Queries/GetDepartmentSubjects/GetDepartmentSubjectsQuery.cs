using MediatR;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.Application.Features.Subjects.Queries.GetDepartmentSubjects
{
    public record GetDepartmentSubjectsQuery : IRequest<IEnumerable<SubjectDto>>
    {
        public int DepartmentId { get; init; }
        public bool? IsActive { get; init; }
    }
}