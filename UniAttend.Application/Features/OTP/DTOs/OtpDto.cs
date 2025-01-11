namespace UniAttend.Application.Features.OTP.DTOs
{
    public record OtpDto
    {
        public string Code { get; init; } = string.Empty;
        public DateTime ExpiryTime { get; init; }
        public bool IsUsed { get; init; }
    }
}