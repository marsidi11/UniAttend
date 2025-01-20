namespace UniAttend.Application.Features.Professors.DTOs
{
    public record ProfessorDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public int DepartmentId { get; init; }
        public string DepartmentName { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}