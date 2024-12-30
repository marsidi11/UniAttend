using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents an alert for student absences that tracks attendance percentage and notification status.
    /// </summary>
    /// <remarks>
    /// This class is used to monitor and manage alerts for students who have exceeded certain absence thresholds.
    /// </remarks>
    public class AbsenceAlert : Entity
    {
        public AbsenceAlert(int studentId, int groupId, decimal absencePercentage)
        {
            StudentId = studentId;
            GroupId = groupId;
            AbsencePercentage = absencePercentage;
            CreatedAt = DateTime.UtcNow;
        }

        public int StudentId { get; }
        public Student Student { get; private set; }
        public StudyGroup Group { get; private set; }
        public int GroupId { get; }
        public decimal AbsencePercentage { get; }
        public bool EmailSent { get; set; } = false;
        public DateTime CreatedAt { get; }

        public void MarkAsSent()
        {
            EmailSent = true;
        }
    }
}