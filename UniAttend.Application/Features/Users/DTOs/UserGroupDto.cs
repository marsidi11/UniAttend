using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.DTOs
{
    public record UserGroupDto
    {
        public int GroupId { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string AcademicYearName { get; init; } = string.Empty;
    }
}