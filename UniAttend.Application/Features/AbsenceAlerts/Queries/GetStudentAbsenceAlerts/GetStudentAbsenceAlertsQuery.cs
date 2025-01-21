using MediatR;
using UniAttend.Application.Features.AbsenceAlerts.DTOs;

namespace UniAttend.Application.Features.AbsenceAlerts.Queries.GetStudentAbsenceAlerts
{
    public record GetStudentAbsenceAlertsQuery : IRequest<IEnumerable<AbsenceAlertDto>>
    {
        public int? StudentId { get; init; }
        public int? StudyGroupId { get; init; }
        public bool? UnsentOnly { get; init; }
    }
}