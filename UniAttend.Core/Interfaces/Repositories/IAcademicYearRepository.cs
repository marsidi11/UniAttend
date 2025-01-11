using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IAcademicYearRepository : IRepository<AcademicYear>
    {
        Task<AcademicYear?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<AcademicYear?> GetCurrentAsync(CancellationToken cancellationToken = default);
        Task<bool> HasOverlappingDatesAsync(DateTime startDate, DateTime endDate, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<AcademicYear>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<AcademicYear?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    }
}