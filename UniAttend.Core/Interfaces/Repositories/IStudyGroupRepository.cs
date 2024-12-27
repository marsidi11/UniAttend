using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IStudyGroupRepository : IRepository<StudyGroup>
    {
        Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default);
    }
}