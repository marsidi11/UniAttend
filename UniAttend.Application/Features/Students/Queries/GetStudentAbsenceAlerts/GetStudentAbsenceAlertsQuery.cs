using MediatR;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentAbsenceAlerts
{
    public record GetStudentAbsenceAlertsQuery : IRequest<IEnumerable<StudentAbsenceAlertDto>>
    {
        public int StudentId { get; init; }
    }
}