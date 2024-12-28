using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents a many-to-many relationship between StudyGroup and Student entities.
    /// </summary>
    public class GroupStudent : Entity
{
    public GroupStudent(int groupId, int studentId)
    {
        GroupId = groupId;
        StudentId = studentId;
    }

    public int GroupId { get; private set; }
    public int StudentId { get; private set; }
    public virtual StudyGroup? Group { get; private set; }
    public virtual Student? Student { get; private set; }
}
}