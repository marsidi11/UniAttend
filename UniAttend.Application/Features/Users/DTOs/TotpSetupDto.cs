namespace UniAttend.Application.Features.Users.DTOs
{
    public record TotpSetupDto(
        string SecretKey,
        string QrCodeUri
    );
}