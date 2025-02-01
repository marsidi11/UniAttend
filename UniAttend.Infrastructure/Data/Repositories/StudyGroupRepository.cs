using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Stats;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class StudyGroupRepository : BaseRepository<StudyGroup>, IStudyGroupRepository
    {
        public StudyGroupRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves a study group with its students and their associated users.
        /// </summary>
        public async Task<StudyGroup?> GetWithStudentsAsync(
            int id, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Students)
                    .ThenInclude(gs => gs.Student)
                        .ThenInclude(s => s!.User)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        /// <summary>
        /// Retrieves study groups by professor ID.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(
            int professorId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Where(g => g.ProfessorId == professorId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves study groups by subject ID.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(
            int subjectId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Where(g => g.SubjectId == subjectId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Checks if a student exists in a specific study group.
        /// </summary>
        public async Task<bool> HasStudentAsync(
            int studyGroupId, 
            int studentId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .AnyAsync(g => g.Id == studyGroupId &&
                    g.Students.Any(s => s.StudentId == studentId),
                    cancellationToken);

        /// <summary>
        /// Retrieves a study group with its schedule.
        /// </summary>
        public async Task<StudyGroup?> GetWithScheduleAsync(
            int id, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Schedules)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        /// <summary>
        /// Calculates attendance statistics for a study group.
        /// </summary>
        public async Task<AttendanceStats> GetAttendanceStatsAsync(
            int studyGroupId, 
            CancellationToken cancellationToken = default)
        {
            var studyGroup = await DbSet
                .Include(g => g.Students)
                .Include(g => g.AttendanceRecords)
                .FirstOrDefaultAsync(g => g.Id == studyGroupId, cancellationToken);

            if (studyGroup == null)
                throw new NotFoundException($"StudyGroup with ID {studyGroupId} not found");

            return new AttendanceStats
            {
                TotalStudents = studyGroup.Students.Count,
                AverageAttendance = studyGroup.AttendanceRecords.Count > 0
                    ? (decimal)studyGroup.AttendanceRecords.Average(r => r.IsConfirmed ? 1 : 0) * 100m
                    : 0m
            };
        }

        /// <summary>
        /// Retrieves study groups by department ID with optional academic year filtering.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetByDepartmentIdAsync(
            int departmentId,
            int? academicYearId = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(g => g.Subject)
                .Include(g => g.Students)
                .Where(g => g.Subject.DepartmentId == departmentId);

            if (academicYearId.HasValue)
                query = query.Where(g => g.AcademicYearId == academicYearId.Value);

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves all study groups with detailed information, optionally filtered by academic year.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetAllWithDetailsAsync(
            int? academicYearId = null, 
            CancellationToken cancellationToken = default)
        {
            IQueryable<StudyGroup> query = DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                    .ThenInclude(p => p!.User)
                .Include(g => g.AcademicYear)
                .Include(g => g.Students)
                    .ThenInclude(gs => gs.Student)
                        .ThenInclude(s => s!.User);

            if (academicYearId.HasValue)
            {
                query = query.Where(g => g.AcademicYearId == academicYearId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a study group by ID with full details.
        /// </summary>
        public async Task<StudyGroup?> GetByIdWithDetailsAsync(
            int id, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Students)
                    .ThenInclude(gs => gs.Student)
                        .ThenInclude(s => s.User)
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        /// <summary>
        /// Checks if a study group exists with the given name, subject ID, and academic year ID.
        /// </summary>
        public async Task<bool> ExistsWithNameAsync(
            string name, 
            int subjectId, 
            int academicYearId, 
            CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(g =>
                g.Name == name &&
                g.SubjectId == subjectId &&
                g.AcademicYearId == academicYearId,
                cancellationToken);

        /// <summary>
        /// Retrieves study groups by professor ID with optional academic year filtering.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(
            int professorId, 
            int? academicYearId = null, 
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                    .ThenInclude(p => p!.User)
                .Include(g => g.AcademicYear)
                .Where(g => g.ProfessorId == professorId);

            if (academicYearId.HasValue)
            {
                query = query.Where(g => g.AcademicYearId == academicYearId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves study groups by student ID.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetByStudentIdAsync(
            int studentId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Students)
                .Include(g => g.Subject)
                .Include(g => g.AcademicYear)
                .Where(g => g.Students.Any(s => s.StudentId == studentId))
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves study groups for a student.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetStudentGroupsAsync(
            int studentId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.AcademicYear)
                .Where(g => g.Students.Any(s => s.StudentId == studentId))
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retrieves study groups for a professor.
        /// </summary>
        public async Task<IEnumerable<StudyGroup>> GetProfessorStudyGroupsAsync(
            int professorId, 
            CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.AcademicYear)
                .Where(g => g.ProfessorId == professorId)
                .ToListAsync(cancellationToken);
    }
}