using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Include(d => d.Subjects.Where(s => s.IsActive))
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Include(d => d.Subjects
                    .Where(s => s.IsActive))
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        }
    
        public async Task<IEnumerable<Department>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(d => d.IsActive)
                .Include(d => d.Subjects
                    .Where(s => s.IsActive))
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .ToListAsync(cancellationToken);
        }
    
        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(d => d.Name == name, cancellationToken);
        }
    }
}