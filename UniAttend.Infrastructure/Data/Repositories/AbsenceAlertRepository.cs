using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class AbsenceAlertRepository : BaseRepository<AbsenceAlert>, IAbsenceAlertRepository
    {
        public AbsenceAlertRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Gets absence alerts by student ID.
        /// </summary>
        public async Task<IEnumerable<AbsenceAlert>> GetByStudentIdAsync(
            int studentId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(a => a.StudyGroup)
                    .ThenInclude(sg => sg.Subject)
                .Where(a => a.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets absence alerts by study group ID.
        /// </summary>
        public async Task<IEnumerable<AbsenceAlert>> GetByGroupIdAsync(
            int studyGroupId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(aa => aa.Student)
                .Include(aa => aa.StudyGroup)
                .Where(aa => aa.StudyGroupId == studyGroupId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets all unsent absence alerts.
        /// </summary>
        public async Task<IEnumerable<AbsenceAlert>> GetUnsentAlertsAsync(
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(aa => aa.Student)
                .Include(aa => aa.StudyGroup)
                .Where(aa => !aa.EmailSent)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Marks an absence alert as sent.
        /// </summary>
        public async Task MarkAlertAsSentAsync(
            int alertId,
            CancellationToken cancellationToken = default)
        {
            var alert = await GetByIdAsync(alertId, cancellationToken);
            if (alert != null)
            {
                alert.MarkAsSent();
                await UpdateAsync(alert, cancellationToken);
            }
        }

        /// <summary>
        /// Checks if there is an active unsent absence alert for the specified student and study group.
        /// </summary>
        public async Task<bool> HasActiveAlertAsync(
            int studentId,
            int studyGroupId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(a =>
                a.StudentId == studentId &&
                a.StudyGroupId == studyGroupId &&
                !a.EmailSent,
                cancellationToken);
        }
    }
}