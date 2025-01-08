using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Course : Entity
    {
        private readonly List<AttendanceRecord> _attendanceRecords = new();
        
        private Course() { } // For domain events/serialization

        public Course(string name, string description, DateTime startTime, DateTime endTime, 
            string location, int professorId, int studyGroupId)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateTimeRange(startTime, endTime);
            ValidateLocation(location);
            
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            Location = location;
            ProfessorId = professorId;
            StudyGroupId = studyGroupId;
            IsActive = true;
        }

        // Identity properties - immutable
        public string Name { get; }
        public string Description { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public string Location { get; }
        public bool IsActive { get; private set; }
        public int ProfessorId { get; }
        public int StudyGroupId { get; }

        // Domain references without EF Core annotations
        public Professor Professor { get; }
        public StudyGroup StudyGroup { get; }
        public IReadOnlyCollection<AttendanceRecord> AttendanceRecords => _attendanceRecords.AsReadOnly();

        // Domain methods
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Course name cannot be empty");
            if (name.Length > 100)
                throw new DomainException("Course name cannot exceed 100 characters");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Course description cannot be empty");
            if (description.Length > 500)
                throw new DomainException("Course description cannot exceed 500 characters");
        }

        private static void ValidateTimeRange(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new DomainException("Start time must be before end time");
        }

        private static void ValidateLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new DomainException("Course location cannot be empty");
            if (location.Length > 100)
                throw new DomainException("Course location cannot exceed 100 characters");
        }
    }
}