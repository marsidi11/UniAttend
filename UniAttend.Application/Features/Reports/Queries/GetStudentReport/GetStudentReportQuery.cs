using MediatR;
using UniAttend.Application.Features.Reports.DTOs;

namespace UniAttend.Application.Features.Reports.Queries.GetStudentReport
{
    public record GetStudentReportQuery : IRequest<StudentReportDto>
    {
        public int StudentId { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}