using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing AcademicYear entities.
    /// </summary>
    public class AcademicYearRepository : BaseRepository<AcademicYear>, IAcademicYearRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcademicYearRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AcademicYearRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves an AcademicYear by its identifier, including study groups.
        /// </summary>
        public override async Task<AcademicYear?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(ay => ay.StudyGroups)
                .FirstOrDefaultAsync(ay => ay.Id == id, cancellationToken);

        /// <summary>
        /// Gets an AcademicYear by name.
        /// </summary>
        public async Task<AcademicYear?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(ay => ay.StudyGroups)
                .FirstOrDefaultAsync(ay => ay.Name == name, cancellationToken);

        /// <summary>
        /// Gets the current active AcademicYear.
        /// </summary>
        public async Task<AcademicYear?> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(ay => ay.StudyGroups)
                .FirstOrDefaultAsync(ay => ay.IsActive, cancellationToken);
        }

        /// <summary>
        /// Checks for overlapping dates with other AcademicYears.
        /// </summary>
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

        /// <summary>
        /// Gets all active AcademicYears.
        /// </summary>
        public async Task<IEnumerable<AcademicYear>> GetActiveAsync(CancellationToken cancellationToken = default)
            => await DbSet
                .Where(ay => ay.IsActive)
                .Include(ay => ay.StudyGroups)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves an AcademicYear by id with detailed study group info.
        /// </summary>
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