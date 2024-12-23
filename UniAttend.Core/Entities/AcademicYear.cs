using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents an academic year with a name, start date, end date, and associated study groups.
    /// </summary>
    public class AcademicYear : ActiveEntity
    {
        private AcademicYear() 
        {
            Name = string.Empty;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            StudyGroups = new List<StudyGroup>();
        }

        public AcademicYear(string name, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date");
            if (name.Length > 20)
                throw new ArgumentException("Name cannot exceed 20 characters", nameof(name));

            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            StudyGroups = new List<StudyGroup>();
        }

        public string Name { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public ICollection<StudyGroup> StudyGroups { get; init; }
    }
}