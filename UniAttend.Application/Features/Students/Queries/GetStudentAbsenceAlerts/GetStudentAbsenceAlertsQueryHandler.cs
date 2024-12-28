using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Students.DTOs;


namespace UniAttend.Application.Features.Students.Queries.GetStudentAbsenceAlerts
{
    public class GetStudentAbsenceAlertsQueryHandler 
        : IRequestHandler<GetStudentAbsenceAlertsQuery, IEnumerable<StudentAbsenceAlertDto>>
    {
        private readonly IAbsenceAlertRepository _absenceAlertRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;

        public GetStudentAbsenceAlertsQueryHandler(
            IAbsenceAlertRepository absenceAlertRepository,
            IStudyGroupRepository studyGroupRepository)
        {
            _absenceAlertRepository = absenceAlertRepository;
            _studyGroupRepository = studyGroupRepository;
        }

        public async Task<IEnumerable<StudentAbsenceAlertDto>> Handle(
            GetStudentAbsenceAlertsQuery request, 
            CancellationToken cancellationToken)
        {
            var alerts = await _absenceAlertRepository.GetByStudentIdAsync(
                request.StudentId, 
                cancellationToken);

            return await Task.WhenAll(alerts.Select(async a =>
            {
                var group = await _studyGroupRepository.GetByIdAsync(a.GroupId, cancellationToken);
                return new StudentAbsenceAlertDto
                {
                    GroupId = a.GroupId,
                    SubjectName = group?.Subject.Name ?? "Unknown",
                    AbsencePercentage = a.AbsencePercentage,
                    EmailSent = a.EmailSent,
                    AlertDate = a.CreatedAt
                };
            }));
        }
    }
}