namespace UniAttend.Application.Features.Students.DTOs
{
    public class StudentGroupDto
    {
        public int StudyGroupId { get; init; }
        public string StudyGroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string AcademicYear { get; init; } = string.Empty;
        public string ProfessorName { get; init; } = string.Empty;
    }
}