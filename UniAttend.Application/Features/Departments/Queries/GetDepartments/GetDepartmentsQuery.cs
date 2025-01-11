using MediatR;
using UniAttend.Application.Features.Departments.DTOs;

namespace UniAttend.Application.Features.Departments.Queries.GetDepartments
{
    public record GetDepartmentsQuery : IRequest<IEnumerable<DepartmentDto>>;
}