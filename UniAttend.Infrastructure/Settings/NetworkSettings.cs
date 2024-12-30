namespace UniAttend.Infrastructure.Settings
{
    public class NetworkSettings
    {
        public string[] AllowedIpRanges { get; set; } = Array.Empty<string>();
        public string[] AllowedMacAddresses { get; set; } = Array.Empty<string>();
        public LocationBoundary CampusBoundary { get; set; } = new();
    }

    public class LocationBoundary
    {
        public double NorthLatitude { get; set; }
        public double SouthLatitude { get; set; }
        public double EastLongitude { get; set; }
        public double WestLongitude { get; set; }
    }
}