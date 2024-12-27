using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Course>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default);
    }
}