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
        IAcademicYearRepository AcademicYears { get; }
        ISubjectRepository Subjects { get; }
        IReportRepository Reports { get; }
        IStudyGroupRepository StudyGroups { get; }
        ICourseSessionRepository CourseSessions { get; }
        IClassroomRepository Classrooms { get; }
        IAbsenceAlertRepository AbsenceAlerts { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}