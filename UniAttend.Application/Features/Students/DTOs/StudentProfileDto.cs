namespace UniAttend.Application.Features.Students.DTOs
{
    /// <summary>
    /// Data transfer object representing a student's profile.
    /// </summary>
    public record StudentProfileDto(
        int Id,
        string StudentId,
        string CardId,
        string FullName,
        string Department,
        IEnumerable<GroupDto> Groups
    );
}