namespace UniAttend.Core.Interfaces.Services
{
    public interface INetworkValidationService
    {
        Task<bool> ValidateDeviceNetworkAsync(string macAddress);
        Task<bool> IsOnCampusNetworkAsync(string ipAddress);
        Task<bool> ValidateLocationAsync(double latitude, double longitude);
    }
}