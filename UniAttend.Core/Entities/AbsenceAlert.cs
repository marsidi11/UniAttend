using UniAttend.Core.Entities.Base;

/// <summary>
/// Represents an alert for student absences that tracks attendance percentage and notification status.
/// </summary>
/// <remarks>
/// This class is used to monitor and manage alerts for students who have exceeded certain absence thresholds.
/// </remarks>

namespace UniAttend.Core.Entities
{
    public class AbsenceAlert : Entity
    {
        public AbsenceAlert(int studentId, int groupId, decimal absencePercentage)
        {
            StudentId = studentId;
            GroupId = groupId;
            AbsencePercentage = absencePercentage;
        }
        public int StudentId { get; }
        public int GroupId { get; }
        public decimal AbsencePercentage { get; }
        public bool EmailSent { get; set; } = false;
    }
}