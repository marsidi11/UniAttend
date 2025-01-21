using MediatR;
using UniAttend.Application.Features.AbsenceAlerts.DTOs;

namespace UniAttend.Application.Features.AbsenceAlerts.Commands.GenerateAbsenceAlert
{
    public record GenerateAbsenceAlertCommand : IRequest<AbsenceAlertDto>
    {
        public int StudentId { get; init; }
        public int StudyGroupId { get; init; }
        public decimal AbsencePercentage { get; init; }
    }
}