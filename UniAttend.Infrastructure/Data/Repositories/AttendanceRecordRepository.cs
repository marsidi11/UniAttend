using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing attendance records in the database.
    /// Handles CRUD operations and specialized queries for attendance tracking.
    /// </summary>
    public class AttendanceRecordRepository : IAttendanceRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceRecord> AddAsync(AttendanceRecord record, CancellationToken cancellationToken = default)
        {
            await _context.Set<AttendanceRecord>().AddAsync(record, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return record;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>().ToListAsync(cancellationToken);

        public async Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>().FindAsync(new object[] { id }, cancellationToken);

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>().AnyAsync(x => x.Id == id, cancellationToken);

        public async Task UpdateAsync(AttendanceRecord record, CancellationToken cancellationToken = default)
        {
            _context.Set<AttendanceRecord>().Update(record);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var record = await GetByIdAsync(id, cancellationToken);
            if (record != null)
            {
                _context.Set<AttendanceRecord>().Remove(record);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<AttendanceRecord>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>()
                .Where(a => a.CourseId == courseId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>()
                .Where(a => a.StudentId == studentId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>()
                .Where(a => a.CheckInTime >= startDate && a.CheckInTime <= endDate)
                .ToListAsync(cancellationToken);

        public async Task<AttendanceRecord?> GetStudentAttendanceForCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>()
                .FirstOrDefaultAsync(a => a.StudentId == studentId && a.CourseId == courseId, cancellationToken);

        public async Task<double> GetStudentAttendancePercentageAsync(int studentId, int groupId, CancellationToken cancellationToken = default)
        {
            var totalCourses = await _context.Set<Course>()
                .CountAsync(c => c.StudyGroupId == groupId && c.IsActive, cancellationToken);

            if (totalCourses == 0) return 0;

            var attendedClasses = await _context.Set<AttendanceRecord>()
                .CountAsync(a => a.StudentId == studentId && a.IsConfirmed, cancellationToken);

            return (double)attendedClasses / totalCourses * 100;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(int courseId, CancellationToken cancellationToken = default)
            => await _context.Set<AttendanceRecord>()
                .Where(a => a.CourseId == courseId && !a.IsConfirmed)
                .ToListAsync(cancellationToken);

        public async Task ConfirmAttendanceRecordsAsync(int courseId, CancellationToken cancellationToken = default)
        {
            var records = await GetUnconfirmedRecordsAsync(courseId, cancellationToken);
            foreach (var record in records)
            {
                record.IsConfirmed = true;
                _context.Set<AttendanceRecord>().Update(record);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}