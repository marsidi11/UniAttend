using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .ToListAsync(cancellationToken);

        public async Task<Student> AddAsync(Student entity, CancellationToken cancellationToken = default)
        {
            await _context.Students.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Student entity, CancellationToken cancellationToken = default)
        {
            _context.Students.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var student = await GetByIdAsync(id, cancellationToken);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Students.AnyAsync(s => s.Id == id, cancellationToken);

        public async Task<Student?> GetByCardIdAsync(string cardId, CancellationToken cancellationToken = default)
            => await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.CardId == cardId, cancellationToken);

        public async Task<Student?> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
            => await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.StudentId == studentId, cancellationToken);

        public async Task<IEnumerable<Student>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default)
            => await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);

        public async Task<bool> CardIdExistsAsync(string cardId, CancellationToken cancellationToken = default)
            => await _context.Students.AnyAsync(s => s.CardId == cardId, cancellationToken);

        public async Task<bool> StudentIdExistsAsync(string studentId, CancellationToken cancellationToken = default)
            => await _context.Students.AnyAsync(s => s.StudentId == studentId, cancellationToken);
    }
}