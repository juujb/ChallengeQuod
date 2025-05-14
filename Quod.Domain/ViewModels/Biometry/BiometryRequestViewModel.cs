using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;


namespace Quod.Domain
{
    public class BiometryRequestViewModel
    {
        public required IFormFile Image { get; set; }

        public required IFormFile ExpectedBiometry { get; set; } // Em um cenário real a biometria deveria já estar armazenado em um banco de dados ou sistema de arquivos seguro

        public required DateTime CaptureDate { get; set; }

        public required DeviceRequestViewModel DeviceInfo { get; set; }

        [SwaggerIgnore]
        public byte[]? ImageBytes { get; set; }

        [SwaggerIgnore]
        public byte[]? ExpectedBytes { get; set; }

        [SwaggerIgnore]
        public string SourceIp { get; set; } = string.Empty;
    }

    public class DeviceRequestViewModel
    {
        public string? Manufacturer { get; set; } = "Fabricante do Dispositivo";
        public string? Model { get; set; } = "Modelo do Dispositivo";
        public string? OS { get; set; } = "Sistema Operacional";
    }
}
