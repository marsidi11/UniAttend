using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities.Attendance;
using System.Security.Cryptography;

namespace UniAttend.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpCodeRepository _otpRepository;
        private const int OTP_LENGTH = 6;
        private const int OTP_EXPIRY_MINUTES = 10;

        public OtpService(IOtpCodeRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }

        public async Task<OtpCode> GenerateOtpAsync(int studentId, int classId, 
            CancellationToken cancellationToken = default)
        {
            var code = GenerateNumericCode();
            var expiryTime = DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES);
            
            var otpCode = new OtpCode(studentId, classId, code, expiryTime);
            await _otpRepository.AddAsync(otpCode, cancellationToken);
            
            return otpCode;
        }

        public async Task<bool> ValidateOtpAsync(string code, int studentId, 
            int classId, CancellationToken cancellationToken = default)
        {
            var otpCode = await _otpRepository.GetValidCodeAsync(
                code, studentId, classId, cancellationToken);
                
            if (otpCode == null) return false;
            
            otpCode.IsUsed = true;
            await _otpRepository.UpdateAsync(otpCode, cancellationToken);
            return true;
        }

        private string GenerateNumericCode()
        {
            var randomNumber = RandomNumberGenerator.GetInt32(1000000);
            return randomNumber.ToString("D6");
        }
    }
}