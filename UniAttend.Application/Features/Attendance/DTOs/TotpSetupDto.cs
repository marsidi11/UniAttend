namespace UniAttend.Application.Features.Attendance.DTOs
{
    public record TotpSetupDto(
        string SecretKey,
        string QrCodeUri
    );
}