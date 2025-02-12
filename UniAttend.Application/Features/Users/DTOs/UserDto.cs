using UniAttend.Application.Features.Departments.DTOs;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.DTOs
{
    public record UserDto
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public UserRole Role { get; init; }
        public string? DepartmentName { get; init; }
        public IEnumerable<DepartmentDto> Departments { get; set; } = [];
        public bool IsActive { get; init; }
        public string? TotpSecret { get; init; }
        public bool? IsTwoFactorEnabled { get; init; }
        public bool? IsTwoFactorVerified { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}