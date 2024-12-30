using MediatR;

namespace UniAttend.Application.Features.Students.Commands.RegisterStudent
{
    public record RegisterStudentCommand : IRequest<int>
    {
        public string StudentId { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public int DepartmentId { get; init; }
        public string? CardId { get; init; }
    }
}