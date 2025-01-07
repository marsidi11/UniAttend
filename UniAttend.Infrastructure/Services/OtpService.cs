using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities.Attendance;
using System.Security.Cryptography;

namespace UniAttend.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpCodeRepository _otpRepository;
        private readonly IEmailService _emailService;
        private const int OTP_LENGTH = 6;
        private const int OTP_EXPIRY_MINUTES = 10;

        public OtpService(IOtpCodeRepository otpRepository, IEmailService emailService)
        {
            _otpRepository = otpRepository;
            _emailService = emailService;
        }

        public async Task<OtpCode> GenerateOtpAsync(int classId, int studentId, CancellationToken cancellationToken = default)
        {
            var code = GenerateNumericCode();
            var expiryTime = DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES);

            var otpCode = new OtpCode(studentId, classId, code, expiryTime);
            await _otpRepository.AddAsync(otpCode, cancellationToken);

            return otpCode;
        }

        public async Task<bool> ValidateOtpAsync(string code, int classId, int studentId, CancellationToken cancellationToken = default)
        {
            var otpCode = await _otpRepository.GetValidCodeAsync(code, studentId, classId, cancellationToken);

            if (otpCode == null) return false;

            otpCode.IsUsed = true;
            await _otpRepository.UpdateAsync(otpCode, cancellationToken);
            return true;
        }

        public async Task<OtpCode?> GetCurrentOtpAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await _otpRepository.GetCurrentOtpForClassAsync(classId, cancellationToken);
        }

        private string GenerateNumericCode()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] numberBytes = new byte[4];
            rng.GetBytes(numberBytes);
            int number = Math.Abs(BitConverter.ToInt32(numberBytes, 0)) % 1000000;
            return number.ToString("D6");
        }
    }
}