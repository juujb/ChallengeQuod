using Microsoft.Extensions.Options;
using Quod.Domain;

namespace Quod.Infra.Mongo
{
    public class DocumentAnalysisRepository : MongoRepository<DocumentAnalysis>, IDocumentAnalysisRepository
    {
        public override string CollectionName => MongoCollections.DocumentAnalysis;

        public DocumentAnalysisRepository(IOptions<DefaultMongoDbSettings> settings) : base(settings)
        {

        }
    }
}
