using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    public class GroupStudent : Entity
    {
        private GroupStudent() { } // For EF Core

        public GroupStudent(int groupId, int studentId)
        {
            GroupId = groupId;
            StudentId = studentId;
        }

        public int GroupId { get; private set; }
        public int StudentId { get; private set; }

        // Navigation properties
        public StudyGroup Group { get; private set; }
        public Student Student { get; private set; }
    }
}