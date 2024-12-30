namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IStudentRepository Students { get; }
        IProfessorRepository Professors { get; }
        IAttendanceRecordRepository AttendanceRecords { get; }
        ICourseRepository Courses { get; }
        IDepartmentRepository Departments { get; }
        IScheduleRepository Schedules { get; }
        IStudyGroupRepository StudyGroups { get; }
        IOtpCodeRepository OtpCodes { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}