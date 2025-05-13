using Microsoft.AspNetCore.Http;

namespace Quod.Domain
{
    public class BiometryRequestViewModel
    {
        public required IFormFile Image { get; set; }
        public required IFormFile ExpectedBiometry { get; set; } // Em um cenário real o arquivo deveria já estar armazenado em um banco de dados ou sistema de arquivos seguro
        public required DateTime CaptureDate { get; set; }
        public required DeviceRequestViewModel DeviceInfo { get; set; }
        public byte[]? ImageBytes { get; set; }
        public byte[]? ExpectedBytes { get; set; }
    }

    public class DeviceRequestViewModel
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? OS { get; set; }
    }
}
