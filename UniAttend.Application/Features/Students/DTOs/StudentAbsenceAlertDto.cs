namespace UniAttend.Application.Features.Students.DTOs
{
    public class StudentAbsenceAlertDto
    {
        public int StudyGroupId { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public decimal AbsencePercentage { get; init; }
        public bool EmailSent { get; init; }
        public DateTime AlertDate { get; init; }
    }
}