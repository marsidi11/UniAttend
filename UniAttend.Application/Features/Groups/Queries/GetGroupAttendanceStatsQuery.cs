using MediatR;
using UniAttend.Application.Features.Groups.DTOs;

namespace UniAttend.Application.Features.Groups.Queries
{
    public record GetGroupAttendanceStatsQuery : IRequest<AttendanceStatsDto>
    {
        public int GroupId { get; init; }
    }
}