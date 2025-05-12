using Newtonsoft.Json;

namespace Quod.Domain
{
    public class NotificationViewModel
    {
        [JsonProperty("transacaoId")]
        public Guid TransactionId { get; set; }

        [JsonProperty("tipoBiometria")]
        public string BiometryType { get; set; }

        [JsonProperty("tipoFraude")]
        public string FraudType { get; set; }

        [JsonProperty("dataCaptura")]
        public DateTime CaptureDate { get; set; }

        [JsonProperty("dispositivo")]
        public DeviceViewModel Device { get; set; }

        [JsonProperty("canalNotificacao")]
        public List<string> NotificationChannels { get; set; }

        [JsonProperty("notificadoPor")]
        public string NotifiedBy { get; set; }

        [JsonProperty("metadados")]
        public MetadataViewModel Metadata { get; set; }
    }

    public class DeviceViewModel
    {
        [JsonProperty("fabricante")]
        public string Manufacturer { get; set; }

        [JsonProperty("modelo")]
        public string Model { get; set; }

        [JsonProperty("sistemaOperacional")]
        public string OperatingSystem { get; set; }
    }

    public class MetadataViewModel
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("ipOrigem")]
        public string SourceIp { get; set; }
    }
}
