namespace UniAttend.Application.Features.Groups.DTOs
{
    public record StudyGroupDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int SubjectId { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public int AcademicYearId { get; init; }
        public string AcademicYearName { get; init; } = string.Empty;
        public int ProfessorId { get; init; }
        public string ProfessorName { get; init; } = string.Empty;
        public int StudentsCount { get; init; }
        public decimal AttendanceRate { get; init; }
        public bool IsActive { get; init; }
    }
}