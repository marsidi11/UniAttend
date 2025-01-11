using MediatR;
using UniAttend.Application.Features.Departments.DTOs;

namespace UniAttend.Application.Features.Departments.Queries.GetDepartmentById
{
    public record GetDepartmentByIdQuery : IRequest<DepartmentDto>
    {
        public int Id { get; init; }
    }
}