using MediatR;
using UniAttend.Application.Features.Reports.DTOs;

namespace UniAttend.Application.Features.Reports.Queries.GetAttendanceReport
{
    public record GetAttendanceReportQuery : IRequest<AttendanceReportRecordDto>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int? DepartmentId { get; init; }
        public int? SubjectId { get; init; }
        public int? StudyGroupId { get; init; }
    }
}