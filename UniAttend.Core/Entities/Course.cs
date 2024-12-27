using System;
using System.Collections.Generic;
using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Entities
{
    public class Course : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public int ProfessorId { get; set; }
        public int StudyGroupId { get; set; }

        // Navigation properties
        public Professor Professor { get; set; }
        public StudyGroup StudyGroup { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        
        public Course()
        {
            AttendanceRecords = new HashSet<AttendanceRecord>();
        }
    }
}