using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.NetworkInformation;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Settings;

namespace UniAttend.Infrastructure.Services
{
    public class NetworkValidationService : INetworkValidationService
    {
        private readonly NetworkSettings _settings;
        private readonly ILogger<NetworkValidationService> _logger;

        public NetworkValidationService(
            IOptions<NetworkSettings> settings,
            ILogger<NetworkValidationService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> ValidateDeviceNetworkAsync(string macAddress)
        {
            try
            {
                // Validate MAC address format
                if (!PhysicalAddress.TryParse(macAddress.Replace(":", "-"), out var physicalAddress))
                {
                    _logger.LogWarning("Invalid MAC address format: {MacAddress}", macAddress);
                    return false;
                }

                // Check if MAC address is in allowed list
                var normalizedMac = macAddress.ToUpper();
                var isAllowed = _settings.AllowedMacAddresses.Contains(normalizedMac);

                if (!isAllowed)
                {
                    _logger.LogWarning("MAC address not in allowed list: {MacAddress}", macAddress);
                }

                return isAllowed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating device MAC address: {MacAddress}", macAddress);
                return false;
            }
        }

        public async Task<bool> IsOnCampusNetworkAsync(string ipAddress)
        {
            try
            {
                if (!IPAddress.TryParse(ipAddress, out var address))
                {
                    _logger.LogWarning("Invalid IP address format: {IpAddress}", ipAddress);
                    return false;
                }

                foreach (var range in _settings.AllowedIpRanges)
                {
                    var networkParts = range.Split('/');
                    if (networkParts.Length != 2) continue;

                    var networkAddress = IPAddress.Parse(networkParts[0]);
                    var prefixLength = int.Parse(networkParts[1]);

                    if (IsIpInRange(address, networkAddress, prefixLength))
                    {
                        return true;
                    }
                }

                _logger.LogWarning("IP address not in allowed ranges: {IpAddress}", ipAddress);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating IP address: {IpAddress}", ipAddress);
                return false;
            }
        }

        public async Task<bool> ValidateLocationAsync(double latitude, double longitude)
        {
            try
            {
                // Check if coordinates are within campus boundaries
                var isWithinLatitude = latitude >= _settings.CampusBoundary.SouthLatitude && 
                                     latitude <= _settings.CampusBoundary.NorthLatitude;

                var isWithinLongitude = longitude >= _settings.CampusBoundary.WestLongitude && 
                                      longitude <= _settings.CampusBoundary.EastLongitude;

                var isValid = isWithinLatitude && isWithinLongitude;

                if (!isValid)
                {
                    _logger.LogWarning(
                        "Location outside campus boundaries. Latitude: {Latitude}, Longitude: {Longitude}", 
                        latitude, longitude);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error validating location. Latitude: {Latitude}, Longitude: {Longitude}", 
                    latitude, longitude);
                return false;
            }
        }

        private bool IsIpInRange(IPAddress address, IPAddress networkAddress, int prefixLength)
        {
            var mask = IPAddress.Parse(PrefixLengthToSubnetMask(prefixLength));
            var networkAddressBytes = networkAddress.GetAddressBytes();
            var addressBytes = address.GetAddressBytes();
            var maskBytes = mask.GetAddressBytes();

            for (var i = 0; i < networkAddressBytes.Length; i++)
            {
                if ((addressBytes[i] & maskBytes[i]) != (networkAddressBytes[i] & maskBytes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private string PrefixLengthToSubnetMask(int prefixLength)
        {
            var mask = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                if (prefixLength >= 8)
                {
                    mask[i] = 255;
                    prefixLength -= 8;
                }
                else if (prefixLength > 0)
                {
                    mask[i] = (byte)(255 << (8 - prefixLength));
                    prefixLength = 0;
                }
                else
                {
                    mask[i] = 0;
                }
            }
            return $"{mask[0]}.{mask[1]}.{mask[2]}.{mask[3]}";
        }
    }
}