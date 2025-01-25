using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
        public class AcademicYearRepository : BaseRepository<AcademicYear>, IAcademicYearRepository
    {
        public AcademicYearRepository(ApplicationDbContext context) : base(context) { }
    
        public override async Task<AcademicYear?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(ay => ay.StudyGroups)
                .FirstOrDefaultAsync(ay => ay.Id == id, cancellationToken);
    
        public async Task<AcademicYear?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(ay => ay.StudyGroups)
                .FirstOrDefaultAsync(ay => ay.Name == name, cancellationToken);
    
        public async Task<AcademicYear?> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(ay => ay.StudyGroups)
                .FirstOrDefaultAsync(ay => ay.IsActive, cancellationToken);
        }
    
        public async Task<bool> HasOverlappingDatesAsync(
            DateTime startDate, 
            DateTime endDate, 
            int? excludeId = null, 
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(ay => ay.Id != excludeId.Value);
            }
    
            return await query.AnyAsync(ay =>
                (startDate >= ay.StartDate && startDate <= ay.EndDate) ||
                (endDate >= ay.StartDate && endDate <= ay.EndDate) ||
                (startDate <= ay.StartDate && endDate >= ay.EndDate),
                cancellationToken);
        }
    
        public async Task<IEnumerable<AcademicYear>> GetActiveAsync(CancellationToken cancellationToken = default)
            => await DbSet
                .Where(ay => ay.IsActive)
                .Include(ay => ay.StudyGroups)
                .ToListAsync(cancellationToken);

        public async Task<AcademicYear?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(ay => ay.StudyGroups)
                .ThenInclude(g => g.Students)
                    .ThenInclude(gs => gs.Student)
            .Include(ay => ay.StudyGroups)
                .ThenInclude(g => g.Subject)
            .FirstOrDefaultAsync(ay => ay.Id == id, cancellationToken);
    }
}