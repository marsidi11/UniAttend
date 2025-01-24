using OtpNet;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.Infrastructure.Services
{
    public class TotpService : ITotpService
    {
        private const int SecretLength = 20;

        public string GenerateSecretKey()
        {
            var secretKey = KeyGeneration.GenerateRandomKey(SecretLength);
            return Base32Encoding.ToString(secretKey);
        }

        public string GenerateQrCodeUri(string secretKey, string email)
        {
            var issuer = Uri.EscapeDataString("UniAttend");
            var account = Uri.EscapeDataString(email);
            return $"otpauth://totp/{issuer}:{account}?secret={secretKey}&issuer={issuer}";
        }

        public bool VerifyTotp(string secretKey, string code)
        {
            var keyBytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(keyBytes);
            return totp.VerifyTotp(code, out _);
        }
    }
}