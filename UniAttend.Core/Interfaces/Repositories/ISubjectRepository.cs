using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<IEnumerable<Subject>> GetAllAsync(bool? isActive = null, int? departmentId = null, CancellationToken cancellationToken = default);
        Task<Subject?> GetByNameAndDepartmentAsync(string name, int departmentId, CancellationToken cancellationToken = default);
        Task<bool> ExistsInDepartmentAsync(string name, int departmentId, CancellationToken cancellationToken = default);
    }
}