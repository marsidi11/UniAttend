namespace UniAttend.Application.Features.Subjects.DTOs
{
    public record SubjectDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int DepartmentId { get; init; }
        public string DepartmentName { get; init; } = string.Empty;
        public int Credits { get; init; }
        public bool IsActive { get; init; }
        public int GroupsCount { get; init; }
        public int StudentsCount { get; init; }
        public decimal AverageAttendance { get; init; }
    }
}