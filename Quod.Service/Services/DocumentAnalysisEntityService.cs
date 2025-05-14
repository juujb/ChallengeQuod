using Quod.Domain;

namespace Quod.Service
{
    public class DocumentAnalysisEntityService : MongoEntityService<DocumentAnalysis, IDocumentAnalysisRepository>, IDocumentAnalysisEntityService
    {
        public DocumentAnalysisEntityService(IDocumentAnalysisRepository repository) : base(repository)
        {

        }

    }
}
