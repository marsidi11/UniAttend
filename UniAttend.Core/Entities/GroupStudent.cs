using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    /// <summary>
    /// Represents a many-to-many relationship between StudyGroup and Student entities.
    /// </summary>
    public class GroupStudent : Entity
{
    public GroupStudent(int studyGroupId, int studentId)
    {
        StudyGroupId = studyGroupId;
        StudentId = studentId;
    }

    public int StudyGroupId { get; private set; }
    public int StudentId { get; private set; }
    public virtual StudyGroup? StudyGroup { get; private set; }
    public virtual Student? Student { get; private set; }
}
}