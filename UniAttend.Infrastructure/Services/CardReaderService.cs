using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace UniAttend.Infrastructure.Services
{
    public class CardReaderService : ICardReaderService
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IAttendanceRecordService _attendanceService;
        private readonly ILogger<CardReaderService> _logger;

        public CardReaderService(
            IClassroomRepository classroomRepository,
            IAttendanceRecordService attendanceService,
            ILogger<CardReaderService> logger)
        {
            _classroomRepository = classroomRepository;
            _attendanceService = attendanceService;
            _logger = logger;
        }

        public async Task<bool> ValidateReaderDeviceAsync(
            string deviceId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var classroom = await _classroomRepository
                    .GetByReaderDeviceIdAsync(deviceId, cancellationToken);
                
                return classroom != null && !string.IsNullOrEmpty(classroom.ReaderDeviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating reader device {DeviceId}", deviceId);
                return false;
            }
        }

        public async Task ProcessCardReadingAsync(
            string cardId, 
            string deviceId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate reader device
                if (!await ValidateReaderDeviceAsync(deviceId, cancellationToken))
                {
                    throw new InvalidOperationException($"Invalid reader device: {deviceId}");
                }

                // Record attendance using card
                await _attendanceService.RecordCardAttendanceAsync(
                    cardId, 
                    deviceId, 
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing card reading. CardId: {CardId}, DeviceId: {DeviceId}", 
                    cardId, deviceId);
                throw;
            }
        }
    }
}