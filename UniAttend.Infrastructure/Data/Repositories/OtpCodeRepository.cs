using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing OTP (One-Time Password) codes in the database.
    /// Handles CRUD operations and validation of OTP codes.
    /// </summary>
    public class OtpCodeRepository : IOtpCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OtpCode?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.OtpCodes.FindAsync(new object[] { id }, cancellationToken);

        public async Task<IEnumerable<OtpCode>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.OtpCodes.ToListAsync(cancellationToken);

        public async Task<OtpCode> AddAsync(OtpCode entity, CancellationToken cancellationToken = default)
        {
            await _context.OtpCodes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(OtpCode entity, CancellationToken cancellationToken = default)
        {
            _context.OtpCodes.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var code = await GetByIdAsync(id, cancellationToken);
            if (code != null)
            {
                _context.OtpCodes.Remove(code);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.OtpCodes.AnyAsync(c => c.Id == id, cancellationToken);

        public async Task<OtpCode?> GetValidCodeAsync(string code, int studentId, int classId, CancellationToken cancellationToken = default)
            => await _context.OtpCodes
                .FirstOrDefaultAsync(c =>
                    c.Code == code &&
                    c.StudentId == studentId &&
                    c.ClassId == classId &&
                    !c.IsUsed &&
                    c.ExpiryTime > DateTime.UtcNow,
                    cancellationToken);

        public async Task<bool> IsCodeValidAsync(string code, int studentId, int classId, CancellationToken cancellationToken = default)
            => await _context.OtpCodes
                .AnyAsync(c =>
                    c.Code == code &&
                    c.StudentId == studentId &&
                    c.ClassId == classId &&
                    !c.IsUsed &&
                    c.ExpiryTime > DateTime.UtcNow,
                    cancellationToken);
    }
}