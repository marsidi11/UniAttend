using UniAttend.Application.Features.Departments.DTOs;

namespace UniAttend.Application.Features.Professors.DTOs
{
    public record ProfessorDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public IEnumerable<DepartmentDto> Departments { get; init; } = new List<DepartmentDto>();
        public bool IsActive { get; init; }
    }
}