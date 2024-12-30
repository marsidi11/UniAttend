using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Subject?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public async Task<IEnumerable<Subject>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.Department)
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);

        public async Task<Subject?> GetByNameAndDepartmentAsync(string name, int departmentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => 
                    s.Name == name && 
                    s.DepartmentId == departmentId,
                    cancellationToken);

        public async Task<IEnumerable<Subject>> GetActiveAsync(CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.Department)
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsInDepartmentAsync(string name, int departmentId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(s => 
                s.Name == name && 
                s.DepartmentId == departmentId,
                cancellationToken);
    }
}