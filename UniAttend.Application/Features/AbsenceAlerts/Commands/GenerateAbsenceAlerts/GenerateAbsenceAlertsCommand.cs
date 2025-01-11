using MediatR;
using UniAttend.Application.Features.AbsenceAlerts.DTOs;

namespace UniAttend.Application.Features.AbsenceAlerts.Commands.GenerateAbsenceAlert
{
    public record GenerateAbsenceAlertCommand : IRequest<AbsenceAlertDto>
    {
        public int StudentId { get; init; }
        public int GroupId { get; init; }
        public decimal AbsencePercentage { get; init; }
    }
}