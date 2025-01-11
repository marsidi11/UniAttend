using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.DTOs
{
    public record AttendanceStatsDto
    {
        public int TotalClasses { get; init; }
        public int AttendedClasses { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}