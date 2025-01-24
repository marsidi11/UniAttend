using System.Threading.Tasks;

namespace UniAttend.Core.Interfaces.Services
{
    public interface ITotpService
    {
        string GenerateSecretKey();
        string GenerateQrCodeUri(string secretKey, string email);
        bool VerifyTotp(string secretKey, string code);
    }
}