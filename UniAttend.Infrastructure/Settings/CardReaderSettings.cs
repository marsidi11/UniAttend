namespace UniAttend.Infrastructure.Settings
{
    public class CardReaderSettings
    {
        public int ReaderTimeoutSeconds { get; set; } = 30;
        public string[] AllowedReaderIds { get; set; } = Array.Empty<string>();
        public bool RequireReaderValidation { get; set; } = true;
        public int MaxRetryAttempts { get; set; } = 3;
    }
}