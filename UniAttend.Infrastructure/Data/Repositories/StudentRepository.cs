using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
        public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }
    
        public override async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
                
        public async Task<Student?> GetByCardIdAsync(string cardId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.CardId == cardId, cancellationToken);
    
        public async Task<Student?> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.StudentId == studentId, cancellationToken);
    
        public async Task<IEnumerable<Student>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);
    
        public async Task<bool> CardIdExistsAsync(string cardId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(s => s.CardId == cardId, cancellationToken);
    
        public async Task<bool> StudentIdExistsAsync(string studentId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(s => s.StudentId == studentId, cancellationToken);
    }
}