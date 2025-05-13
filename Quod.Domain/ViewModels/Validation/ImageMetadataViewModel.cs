namespace Quod.Domain
{
    public class ImageMetadataViewModel
    {
        public DateTime? CaptureDate { get; set; }
        public DeviceViewModel? DeviceInfo { get; set; }
        public GeoLocationViewModel? Location { get; set; }
        public Dictionary<string, string> RawMetadata { get; set; } = new Dictionary<string, string>();
        public bool IsSuspect { get; set; } = false;
    }
}
