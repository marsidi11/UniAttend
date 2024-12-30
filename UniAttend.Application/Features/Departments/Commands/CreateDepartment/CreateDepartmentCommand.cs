using MediatR;

namespace UniAttend.Application.Features.Departments.Commands.CreateDepartment
{
    public record CreateDepartmentCommand : IRequest<int>
    {
        public string Name { get; init; } = string.Empty;
    }
}