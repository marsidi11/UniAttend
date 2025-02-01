using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing Student entities.
    /// </summary>
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        /// <summary>
        /// Initializes a new instance of the StudentRepository class.
        /// </summary>
        public StudentRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves all students with related details.
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .OrderBy(s => s.User.LastName)
                .ThenBy(s => s.User.FirstName)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a student by its identifier with related details.
        /// </summary>
        public override async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        /// <summary>
        /// Retrieves a student by the provided card ID.
        /// </summary>
        public async Task<Student?> GetByCardIdAsync(string cardId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.CardId == cardId, cancellationToken);

        /// <summary>
        /// Retrieves a student by the provided student ID.
        /// </summary>
        public async Task<Student?> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.StudentId == studentId, cancellationToken);

        /// <summary>
        /// Retrieves students by the related department identifier.
        /// </summary>
        public async Task<IEnumerable<Student>> GetByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Checks if a card ID already exists for any student.
        /// </summary>
        public async Task<bool> CardIdExistsAsync(string cardId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(s => s.CardId == cardId, cancellationToken);

        /// <summary>
        /// Checks if a student ID already exists.
        /// </summary>
        public async Task<bool> StudentIdExistsAsync(string studentId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(s => s.StudentId == studentId, cancellationToken);

        /// <summary>
        /// Retrieves a student by its identifier with details.
        /// </summary>
        public async Task<Student?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a student by its identifier including only user details.
        /// </summary>
        public async Task<Student?> GetByIdWithUserAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}