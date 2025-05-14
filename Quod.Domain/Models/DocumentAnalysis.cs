using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Quod.Domain
{
    public class DocumentAnalysis : Entity
    {
        [BsonElement("valido")]
        public bool IsValid { get; set; }

        [BsonElement("errosDeValidacao")]
        public List<string> ValidationErrors { get; set; } = new ();

        [BsonElement("detalhesDaValidacao")]
        public DocumentAnalysisDetails? AnalysisDetails { get; set; }
    }

    public class DocumentAnalysisDetails
    {
        [BsonElement("pontuacaoDeMatchingFacial")]
        public double? FaceMatchScore { get; set; }

        [BsonElement("textoDoDocumento")]
        public string DocumentText { get; set; }
    }
}
