using UniAttend.Core.Entities.Attendance;

/// <summary>
/// Provides services for managing card reader devices and processing card reading operations.
/// </summary>
namespace UniAttend.Core.Interfaces.Services
{
    public interface ICardReaderService
    {
        Task<bool> ValidateReaderDeviceAsync(string deviceId, 
            CancellationToken cancellationToken = default);
        Task ProcessCardReadingAsync(string cardId, string deviceId, 
            CancellationToken cancellationToken = default);
    }
}