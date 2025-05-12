using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Quod.Domain
{
    public class Notification : Entity
    {
        [BsonElement("transacaoId")]
        public Guid TransactionId { get; set; }

        [BsonElement("tipoBiometria")]
        public string BiometryType { get; set; }

        [BsonElement("tipoFraude")]
        public string FraudType { get; set; }

        [BsonElement("dataCaptura")]
        public DateTime CaptureDate { get; set; }

        [BsonElement("dispositivo")]
        public Device Device { get; set; }

        [BsonElement("canalNotificacao")]
        public List<string> NotificationChannels { get; set; }

        [BsonElement("notificadoPor")]
        public string NotifiedBy { get; set; }

        [BsonElement("metadados")]
        public Metadata Metadata { get; set; }
    }

    public class Device
    {
        [BsonElement("fabricante")]
        public string Manufacturer { get; set; }

        [BsonElement("modelo")]
        public string Model { get; set; }

        [BsonElement("sistemaOperacional")]
        public string OperatingSystem { get; set; }
    }

    public class Metadata
    {
        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }

        [BsonElement("ipOrigem")]
        public string SourceIp { get; set; }
    }
}
