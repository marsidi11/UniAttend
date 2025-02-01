using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Settings;
using System.Net.Sockets;

namespace UniAttend.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for network validation logic.
    /// </summary>
    public class NetworkValidationService : INetworkValidationService
    {
        private readonly NetworkSettings _settings;
        private readonly ILogger<NetworkValidationService> _logger;
        private readonly ICourseSessionRepository _courseSessionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkValidationService"/> class.
        /// </summary>
        public NetworkValidationService(
            IOptions<NetworkSettings> settings,
            ILogger<NetworkValidationService> logger,
            ICourseSessionRepository courseSessionRepository)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _courseSessionRepository = courseSessionRepository ?? throw new ArgumentNullException(nameof(courseSessionRepository));
        }

        /// <inheritdoc/>
        public bool IsOnAllowedNetwork(string clientIp)
        {
            try
            {
                if (string.IsNullOrEmpty(clientIp))
                {
                    _logger.LogWarning("Client IP address is null or empty");
                    return false;
                }

                if (!IPAddress.TryParse(clientIp, out var ipAddress))
                {
                    _logger.LogWarning("Failed to parse client IP address: {ClientIp}", clientIp);
                    return false;
                }

                if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ipAddress = ipAddress.MapToIPv4();
                }

                var subnet = IPNetwork.Parse(_settings.ClassroomSubnet);
                return subnet.Contains(ipAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating client IP {ClientIp}", clientIp);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> IsOnClassroomNetworkAsync(string clientIp, int courseSessionId)
        {
            try
            {
                var session = await _courseSessionRepository.GetByIdWithClassroomAsync(courseSessionId);
                if (session?.Classroom == null)
                {
                    _logger.LogWarning("Course session {SessionId} or classroom not found", courseSessionId);
                    return false;
                }

                // First check if client is on allowed network
                if (!IsOnAllowedNetwork(clientIp))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating classroom network for session {SessionId}", courseSessionId);
                return false;
            }
        }
    }
}