using Microsoft.AspNetCore.Http;

namespace Quod.Domain
{
    public class BiometryUploadRequest
    {
        public IFormFile? ImageFile { get; set; }
        public string? DeviceManufacturer { get; set; }
        public string? DeviceModel { get; set; }
        public string? OS { get; set; }
        public DateTime CaptureDate { get; set; }
    }
}
