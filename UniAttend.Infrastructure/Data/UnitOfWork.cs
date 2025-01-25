using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories;

namespace UniAttend.Infrastructure.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        
        // Repositories are lazily initialized
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
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Thread-safe lazy initialization using null-coalescing operator
        public IUserRepository Users => 
            _users ??= new UserRepository(_context);

        public IStudentRepository Students =>
            _students ??= new StudentRepository(_context);

        public IProfessorRepository Professors =>
            _professors ??= new ProfessorRepository(_context);

        public IAttendanceRecordRepository AttendanceRecords =>
            _attendanceRecords ??= new AttendanceRecordRepository(_context);

        public IDepartmentRepository Departments =>
            _departments ??= new DepartmentRepository(_context);
        
        public IScheduleRepository Schedules =>
            _schedules ??= new ScheduleRepository(_context);

        public IStudyGroupRepository StudyGroups =>
            _studyGroups ??= new StudyGroupRepository(_context);

        public IAcademicYearRepository AcademicYears =>
            _academicYears ??= new AcademicYearRepository(_context);

        public ISubjectRepository Subjects =>
            _subjects ??= new SubjectRepository(_context);

        public IReportRepository Reports =>
            _reports ??= new ReportRepository(_context);
        
        public ICourseSessionRepository CourseSessions =>
            _courseSessions ??= new CourseSessionRepository(_context);

        public IClassroomRepository Classrooms =>
            _classrooms ??= new ClassroomRepository(_context);
        
        public IAbsenceAlertRepository AbsenceAlerts =>
            _absenceAlerts ??= new AbsenceAlertRepository(_context);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

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

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}