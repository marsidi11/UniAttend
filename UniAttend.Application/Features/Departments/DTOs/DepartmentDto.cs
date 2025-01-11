using UniAttend.Core.Entities;

namespace UniAttend.Application.Features.Departments.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public int SubjectsCount { get; init; }
        public int StudentsCount { get; init; }
        public int ProfessorsCount { get; init; }
    }
}