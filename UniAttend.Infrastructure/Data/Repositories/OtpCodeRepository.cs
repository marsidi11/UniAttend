using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing OTP (One-Time Password) codes in the database.
    /// Handles CRUD operations and validation of OTP codes.
    /// </summary>
    public class OtpCodeRepository : BaseRepository<OtpCode>, IOtpCodeRepository
    {
        public OtpCodeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<OtpCode?> GetValidCodeAsync(
            string code,
            int studentId,
            int classId,
            CancellationToken cancellationToken = default)
            => await DbSet.FirstOrDefaultAsync(c =>
                c.Code == code &&
                c.StudentId == studentId &&
                c.ClassId == classId &&
                !c.IsUsed &&
                c.ExpiryTime > DateTime.UtcNow,
                cancellationToken);

        public async Task<bool> IsCodeValidAsync(
            string code,
            int studentId,
            int classId,
            CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(c =>
                c.Code == code &&
                c.StudentId == studentId &&
                c.ClassId == classId &&
                !c.IsUsed &&
                c.ExpiryTime > DateTime.UtcNow,
                cancellationToken);

        public async Task<OtpCode?> GetCurrentOtpForClassAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(c => c.ClassId == classId &&
                           !c.IsUsed &&
                           c.ExpiryTime > DateTime.UtcNow)
                .OrderByDescending(c => c.ExpiryTime)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}