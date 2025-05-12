namespace Quod.Domain
{
    public class ImageMetadata
    {
        public DateTime? CaptureDate { get; set; }
        public DeviceViewModel? DeviceInfo { get; set; }
        public GeoLocation? Location { get; set; }
        public Dictionary<string, string> RawMetadata { get; set; } = new Dictionary<string, string>();
        public bool IsSuspect { get; set; } = false;
    }
}
