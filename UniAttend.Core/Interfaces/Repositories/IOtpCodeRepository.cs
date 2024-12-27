using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IOtpCodeRepository : IRepository<OtpCode>
    {
        Task<OtpCode?> GetValidCodeAsync(string code, int studentId, int classId, CancellationToken cancellationToken = default);
        Task<bool> IsCodeValidAsync(string code, int studentId, int classId, CancellationToken cancellationToken = default);
    }
}