using System;

namespace UniAttend.Infrastructure.Settings
{
    /// <summary>
    /// Configuration settings for network validation.
    /// </summary>
    public class NetworkSettings
    {
        /// <summary>
        /// Gets or sets the subnet range for classroom network (e.g. "192.168.1.0/24").
        /// </summary>
        public string ClassroomSubnet { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the allowed IP range for client connections.
        /// </summary>
        public string AllowedIpRange { get; set; } = string.Empty;
    }
}