using MediatR;
using UniAttend.Application.Features.Reports.DTOs;

namespace UniAttend.Application.Features.Reports.Queries.GetGroupReport
{
    public record GetGroupReportQuery : IRequest<GroupReportDto>
    {
        public int GroupId { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}