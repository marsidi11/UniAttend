namespace UniAttend.Core.Interfaces.Services
{
    /// <summary>
    /// Service for validating network access and restrictions.
    /// </summary>
    public interface INetworkValidationService
    {
        /// <summary>
        /// Validates if the provided client IP is within the allowed network range.
        /// </summary>
        /// <param name="clientIp">The IP address to validate.</param>
        /// <returns>True if the IP is within allowed range, false otherwise.</returns>
        bool IsOnAllowedNetwork(string clientIp);
        
        /// <summary>
        /// Validates if the provided client IP is within the classroom subnet.
        /// </summary>
        /// <param name="clientIp">The IP address to validate.</param>
        /// <param name="courseSessionId">The course session ID to check classroom network.</param>
        /// <returns>True if the IP is within classroom network, false otherwise.</returns>
        Task<bool> IsOnClassroomNetworkAsync(string clientIp, int courseSessionId);
    }
}