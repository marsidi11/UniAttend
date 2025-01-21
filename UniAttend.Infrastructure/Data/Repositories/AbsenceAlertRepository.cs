using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class AbsenceAlertRepository : BaseRepository<AbsenceAlert>, IAbsenceAlertRepository
    {
        public AbsenceAlertRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<AbsenceAlert>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(aa => aa.Student)
                .Include(aa => aa.StudyGroup)
                .Where(aa => aa.StudentId == studentId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AbsenceAlert>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(aa => aa.Student)
                .Include(aa => aa.StudyGroup)
                .Where(aa => aa.StudyGroupId == studyGroupId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AbsenceAlert>> GetUnsentAlertsAsync(CancellationToken cancellationToken = default)
            => await DbSet
                .Include(aa => aa.Student)
                .Include(aa => aa.StudyGroup)
                .Where(aa => !aa.EmailSent)
                .ToListAsync(cancellationToken);

        public async Task MarkAlertAsSentAsync(int alertId, CancellationToken cancellationToken = default)
        {
            var alert = await GetByIdAsync(alertId, cancellationToken);
            if (alert != null)
            {
                alert.MarkAsSent();
                await UpdateAsync(alert, cancellationToken);
            }
        }

        public async Task<bool> HasActiveAlertAsync(int studentId, int studyGroupId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(aa => 
                aa.StudentId == studentId && 
                aa.StudyGroupId == studyGroupId && 
                !aa.EmailSent, 
                cancellationToken);
    }
}