using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.DTOs
{
    public record UserDetailsDto
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public UserRole Role { get; init; }
        public int? DepartmentId { get; init; }
        public string? DepartmentName { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public AttendanceStatsDto? AttendanceStats { get; init; }
        public IEnumerable<UserGroupDto> Groups { get; init; } = new List<UserGroupDto>();
    }
}