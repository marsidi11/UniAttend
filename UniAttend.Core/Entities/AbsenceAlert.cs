using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class AbsenceAlert : Entity
    {
        private AbsenceAlert() { } // For domain events/serialization

        public AbsenceAlert(int studentId, int studyGroupId, decimal absencePercentage)
        {
            ValidateAbsencePercentage(absencePercentage);
            
            StudentId = studentId;
            StudyGroupId = studyGroupId;
            AbsencePercentage = absencePercentage;
            EmailSent = false;
        }

        // Identity properties - immutable
        public int StudentId { get; }
        public int StudyGroupId { get; }
        public decimal AbsencePercentage { get; }
        public bool EmailSent { get; private set; }

        // Domain references without EF Core annotations
        public Student Student { get; }
        public StudyGroup StudyGroup { get; }

        // Domain methods
        public void MarkAsSent() => EmailSent = true;

        // Domain validation
        private void ValidateAbsencePercentage(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new DomainException("Absence percentage must be between 0 and 100");
        }
    }
}