using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for student repository to handle CRUD operations for Student entities.
    /// </summary>
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
        
        Task<Student?> GetByCardIdAsync(string cardId, CancellationToken cancellationToken = default);
        Task<Student?> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Student>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<bool> CardIdExistsAsync(string cardId, CancellationToken cancellationToken = default);
        Task<bool> StudentIdExistsAsync(string studentId, CancellationToken cancellationToken = default);
        Task<Student?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
        Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}