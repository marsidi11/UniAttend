namespace UniAttend.Application.Features.Students.DTOs
{
    /// <summary>
    /// Data transfer object representing a study group.
    /// </summary>
    public record GroupDto(
        int Id,
        string Name,
        string Subject,
        string Professor,
        int AcademicYear
    );
}