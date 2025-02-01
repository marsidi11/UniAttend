using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories;

namespace UniAttend.Infrastructure.Data
{
    /// <summary>
    /// Represents a unit of work for managing repository operations and database transactions.
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        // Repositories are lazily initialized.
        private IUserRepository? _users;
        private IStudentRepository? _students;
        private IProfessorRepository? _professors;
        private IAttendanceRecordRepository? _attendanceRecords;
        private IDepartmentRepository? _departments;
        private IScheduleRepository? _schedules;
        private IAcademicYearRepository? _academicYears;
        private ISubjectRepository? _subjects;
        private IReportRepository? _reports;
        private IStudyGroupRepository? _studyGroups;
        private ICourseSessionRepository? _courseSessions;
        private IClassroomRepository? _classrooms;
        private IAbsenceAlertRepository? _absenceAlerts;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified database context.
        /// </summary>
        /// <param name="context">An instance of <see cref="ApplicationDbContext"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the user repository.
        /// </summary>
        public IUserRepository Users => _users ??= new UserRepository(_context);

        /// <summary>
        /// Gets the student repository.
        /// </summary>
        public IStudentRepository Students => _students ??= new StudentRepository(_context);

        /// <summary>
        /// Gets the professor repository.
        /// </summary>
        public IProfessorRepository Professors => _professors ??= new ProfessorRepository(_context);

        /// <summary>
        /// Gets the attendance record repository.
        /// </summary>
        public IAttendanceRecordRepository AttendanceRecords =>
            _attendanceRecords ??= new AttendanceRecordRepository(_context);

        /// <summary>
        /// Gets the department repository.
        /// </summary>
        public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);

        /// <summary>
        /// Gets the schedule repository.
        /// </summary>
        public IScheduleRepository Schedules => _schedules ??= new ScheduleRepository(_context);

        /// <summary>
        /// Gets the study group repository.
        /// </summary>
        public IStudyGroupRepository StudyGroups => _studyGroups ??= new StudyGroupRepository(_context);

        /// <summary>
        /// Gets the academic year repository.
        /// </summary>
        public IAcademicYearRepository AcademicYears => _academicYears ??= new AcademicYearRepository(_context);

        /// <summary>
        /// Gets the subject repository.
        /// </summary>
        public ISubjectRepository Subjects => _subjects ??= new SubjectRepository(_context);

        /// <summary>
        /// Gets the report repository.
        /// </summary>
        public IReportRepository Reports => _reports ??= new ReportRepository(_context);

        /// <summary>
        /// Gets the course session repository.
        /// </summary>
        public ICourseSessionRepository CourseSessions => _courseSessions ??= new CourseSessionRepository(_context);

        /// <summary>
        /// Gets the classroom repository.
        /// </summary>
        public IClassroomRepository Classrooms => _classrooms ??= new ClassroomRepository(_context);

        /// <summary>
        /// Gets the absence alert repository.
        /// </summary>
        public IAbsenceAlertRepository AbsenceAlerts => _absenceAlerts ??= new AbsenceAlertRepository(_context);

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// Commits the current transaction asynchronously, saving all pending changes.
        /// If an error occurs, the transaction is rolled back.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <exception cref="Exception">Thrown when committing or saving changes fails.</exception>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                if (_transaction is not null)
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        /// Saves all changes made in the context to the underlying database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="UnitOfWork"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Disposes the current instance of the <see cref="UnitOfWork"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}