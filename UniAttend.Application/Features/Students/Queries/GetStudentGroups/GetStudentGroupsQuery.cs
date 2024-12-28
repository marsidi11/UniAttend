using MediatR;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentGroups
{
    public record GetStudentGroupsQuery : IRequest<IEnumerable<StudentGroupDto>>
    {
        public int StudentId { get; init; }
        public int? AcademicYearId { get; init; }
    }
}