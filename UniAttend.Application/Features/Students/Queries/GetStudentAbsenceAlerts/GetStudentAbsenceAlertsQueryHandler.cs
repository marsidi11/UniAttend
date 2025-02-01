using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentAbsenceAlerts
{
    /// <summary>
    /// Handles the query to get student absence alerts.
    /// </summary>
    public class GetStudentAbsenceAlertsQueryHandler : IRequestHandler<GetStudentAbsenceAlertsQuery, IEnumerable<StudentAbsenceAlertDto>>
    {
        private readonly IAbsenceAlertRepository _absenceAlertRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStudentAbsenceAlertsQueryHandler"/> class.
        /// </summary>
        /// <param name="absenceAlertRepository">The absence alert repository.</param>
        /// <param name="studyGroupRepository">The study group repository.</param>
        public GetStudentAbsenceAlertsQueryHandler(
            IAbsenceAlertRepository absenceAlertRepository,
            IStudyGroupRepository studyGroupRepository)
        {
            _absenceAlertRepository = absenceAlertRepository;
            _studyGroupRepository = studyGroupRepository;
        }

        /// <summary>
        /// Handles the query by retrieving absence alerts and associated study group info.
        /// </summary>
        /// <param name="request">The query request containing the student Id.</param>
        /// <param name="cancellationToken">A token to observe cancellation requests.</param>
        /// <returns>An enumerable of <see cref="StudentAbsenceAlertDto"/>.</returns>
        public async Task<IEnumerable<StudentAbsenceAlertDto>> Handle(
            GetStudentAbsenceAlertsQuery request, 
            CancellationToken cancellationToken)
        {
            var alerts = await _absenceAlertRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);

            return await Task.WhenAll(alerts.Select(async a =>
            {
                var studyGroup = await _studyGroupRepository.GetByIdAsync(a.StudyGroupId, cancellationToken);
                return new StudentAbsenceAlertDto
                {
                    StudyGroupId = a.StudyGroupId,
                    SubjectName = studyGroup?.Subject.Name ?? "Unknown",
                    AbsencePercentage = a.AbsencePercentage,
                    EmailSent = a.EmailSent,
                    AlertDate = a.CreatedAt
                };
            }));
        }
    }
}