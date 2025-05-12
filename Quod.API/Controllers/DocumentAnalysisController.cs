using Microsoft.AspNetCore.Mvc;
using Quod.Domain;

[ApiController]
[Route("api/document-analysis")]
public class DocumentAnalysisController : ControllerBase
{
    private readonly IDocumentAnalysisService _documentAnalysisService;

    public DocumentAnalysisController(IDocumentAnalysisService documentAnalysisService)
    {
        _documentAnalysisService = documentAnalysisService ?? throw new ArgumentNullException(nameof(documentAnalysisService));
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<DocumentAnalysisResultViewModel>> AnalyzeDocument([FromForm] DocumentAnalysisRequestViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _documentAnalysisService.AnalyzeDocumentAsync(request);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a análise de documentos: {ex.Message}");
        }
    }
}