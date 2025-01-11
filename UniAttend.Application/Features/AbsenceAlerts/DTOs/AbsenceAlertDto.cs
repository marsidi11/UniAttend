namespace UniAttend.Application.Features.AbsenceAlerts.DTOs
{
    public record AbsenceAlertDto
    {
        public int StudentId { get; init; }
        public int GroupId { get; init; }
        public decimal AbsencePercentage { get; init; }
        public bool EmailSent { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}