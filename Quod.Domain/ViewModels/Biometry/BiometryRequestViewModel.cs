using Microsoft.AspNetCore.Http;

namespace Quod.Domain
{
    public class BiometryRequestViewModel
    {
        public required IFormFile Image { get; set; }
        public required byte[] BiometryData { get; set; }
        public required DateTime CaptureDate { get; set; }
        public required DeviceRequestViewModel DeviceInfo { get; set; }
        public byte[]? ImageFile { get; set; }
    }

    public class DeviceRequestViewModel
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? OS { get; set; }
    }
}
