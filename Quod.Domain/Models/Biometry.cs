using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Quod.Domain
{
    public class Biometry : Entity
    {
        [BsonElement("tipoBiometria")]
        public BiometryType BiometryType { get; set; }

        [BsonElement("dataCaptura")]
        public DateTime CaptureDate { get; set; }

        [BsonElement("dispositivo")]
        public Device DeviceInfo { get; set; }

        [BsonElement("metadadosImagem")]
        public Metadata Metadata { get; set; }

        [BsonElement("qualidadeImagem")]
        public double QualityScore { get; set; }

        [BsonElement("similaridadeImagem")]
        public double SimilarityScore { get; set; }

        [BsonElement("errosValidacao")]
        public List<string> ValidationErrors { get; set; } = new List<string>();

        [BsonElement("valido")]
        public bool IsValid { get; set; }
    }

}
