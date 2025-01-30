namespace UniAttend.Core.Interfaces.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateStudentReportAsync(int studentId, DateTime? startDate, DateTime? endDate);
        Task<byte[]> GenerateGroupReportAsync(int groupId, DateTime? startDate, DateTime? endDate);
        Task<byte[]> GenerateDepartmentReportAsync(int departmentId, int? academicYearId);
        Task<byte[]> GenerateAttendanceReportAsync(int studyGroupId, DateTime startDate, DateTime endDate);
    }
}