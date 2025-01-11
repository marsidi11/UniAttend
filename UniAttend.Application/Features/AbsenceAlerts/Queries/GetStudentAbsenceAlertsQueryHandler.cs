using MediatR;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.AbsenceAlerts.DTOs;

namespace UniAttend.Application.Features.AbsenceAlerts.Queries.GetStudentAbsenceAlerts
{
    public class GetStudentAbsenceAlertsQueryHandler : IRequestHandler<GetStudentAbsenceAlertsQuery, IEnumerable<AbsenceAlertDto>>
    {
        private readonly IAbsenceAlertRepository _absenceAlertRepository;

        public GetStudentAbsenceAlertsQueryHandler(IAbsenceAlertRepository absenceAlertRepository)
        {
            _absenceAlertRepository = absenceAlertRepository;
        }

        public async Task<IEnumerable<AbsenceAlertDto>> Handle(GetStudentAbsenceAlertsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<AbsenceAlert> alerts;

            if (request.UnsentOnly == true)
            {
                alerts = await _absenceAlertRepository.GetUnsentAlertsAsync(cancellationToken);
            }
            else if (request.StudentId.HasValue)
            {
                alerts = await _absenceAlertRepository.GetByStudentIdAsync(request.StudentId.Value, cancellationToken);
            }
            else if (request.GroupId.HasValue)
            {
                alerts = await _absenceAlertRepository.GetByGroupIdAsync(request.GroupId.Value, cancellationToken);
            }
            else
            {
                alerts = await _absenceAlertRepository.GetAllAsync(cancellationToken);
            }

            return alerts.Select(a => new AbsenceAlertDto
            {
                StudentId = a.StudentId,
                GroupId = a.GroupId,
                AbsencePercentage = a.AbsencePercentage,
                EmailSent = a.EmailSent,
                CreatedAt = a.CreatedAt
            });
        }
    }
}