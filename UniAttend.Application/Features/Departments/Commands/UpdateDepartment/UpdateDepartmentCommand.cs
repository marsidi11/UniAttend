using MediatR;

namespace UniAttend.Application.Features.Departments.Commands.UpdateDepartment
{
    public record UpdateDepartmentCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}