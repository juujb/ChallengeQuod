namespace Quod.Domain
{
    public interface IDocumentAnalysisService
    {
        Task<DocumentAnalysisResultViewModel> AnalyzeDocumentAsync(DocumentAnalysisRequestViewModel request);
    }
}
