namespace UniAttend.Application.Features.Users.DTOs
{
    public record UserGroupDto
    {
        public int StudyGroupId { get; init; }
        public string StudyGroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string AcademicYearName { get; init; } = string.Empty;
        public string ProfessorName { get; init; } = string.Empty;
    }
}