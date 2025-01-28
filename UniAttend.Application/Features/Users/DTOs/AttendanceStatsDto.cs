using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.DTOs
{
    public record AttendanceStatsDto
    {
        public int TotalCourseSessions { get; init; }
        public int AttendedCourseSessions { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}