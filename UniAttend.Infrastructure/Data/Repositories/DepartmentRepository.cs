using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Departments
                .Include(d => d.Subjects)
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Departments
                .Include(d => d.Subjects)
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .ToListAsync(cancellationToken);

        public async Task<Department> AddAsync(Department entity, CancellationToken cancellationToken = default)
        {
            await _context.Departments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Department entity, CancellationToken cancellationToken = default)
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await GetByIdAsync(id, cancellationToken);
            if (department != null)
            {
                department.Deactivate();
                await UpdateAsync(department, cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Departments.AnyAsync(d => d.Id == id, cancellationToken);

        public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == name, cancellationToken);

        public async Task<IEnumerable<Department>> GetActiveAsync(CancellationToken cancellationToken = default)
            => await _context.Departments
                .Where(d => d.IsActive)
                .Include(d => d.Subjects)
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .ToListAsync(cancellationToken);

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
            => await _context.Departments.AnyAsync(d => d.Name == name, cancellationToken);
    }
}