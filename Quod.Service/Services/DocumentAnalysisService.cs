using Quod.Domain;

namespace Quod.Service
{
    public class DocumentAnalysisService : IDocumentAnalysisService
    {
        private readonly IImageValidator _imageValidator;
        private readonly IImageCompareService _imageCompareService;
        private readonly IDocumentAnalysisEntityService _documentAnalysisEntityService;

        public DocumentAnalysisService(
            IImageValidator imageValidator,
            IImageCompareService imageCompareService,
            IDocumentAnalysisEntityService documentAnalysisEntityService)
        {
            _imageValidator = imageValidator ?? throw new ArgumentNullException(nameof(imageValidator));
            _imageCompareService = imageCompareService ?? throw new ArgumentNullException(nameof(imageCompareService));
            _documentAnalysisEntityService = documentAnalysisEntityService ?? throw new ArgumentNullException(nameof(documentAnalysisEntityService));
        }

        public async Task<DocumentAnalysisResultViewModel> AnalyzeDocumentAsync(DocumentAnalysisRequestViewModel request) // Algumas simulações de validação foram feitas com Random() para simular a lógica de validação real.
        {
            var result = new DocumentAnalysisResultViewModel();

            request.DocumentImage = await request.DocumentImageFile.ConvertToByteArrayAsync();
            request.FaceImage = await request.DocumentImageFile.ConvertToByteArrayAsync();

            var documentImageValidation = await _imageValidator.ValidateImageAsync(request.DocumentImage, ImageType.Document);
            if (!documentImageValidation.IsValid)
            {
                result.IsValid = false;
                result.ValidationErrors.AddRange(documentImageValidation.Errors);
                return result;
            }

            var faceImageValidation = await _imageValidator.ValidateImageAsync(request.FaceImage, ImageType.Facial);
            if (!faceImageValidation.IsValid)
            {
                result.IsValid = false;
                result.ValidationErrors.AddRange(faceImageValidation.Errors);
                return result;
            }

            var (isSimilar, faceMatchScore) = _imageCompareService.IsImageSimilar(request.DocumentImage, request.FaceImage, 0.8); // Em um caso real, deveria ser verificado apenas o rosto contido em um trecho do documento (ex: RG ou CNH)
            if (!isSimilar)
            {
                result.IsValid = false;
                result.ValidationErrors.Add($"A pontuação de similaridade da face é baixa: {faceMatchScore:P}");
            }

            string documentText = "Texto extraído do RG"; // Aqui deveria ser implementada a lógica de extração de texto do documento, utilizando OCR ou outra técnica.

            result.AnalysisDetails = new DocumentAnalysisDetailsViewModel
            {
                FaceMatchScore = faceMatchScore,
                DocumentText = documentText
            };

            var entity = MapRequestToDocumentAnalysisEntity(result);

            await _documentAnalysisEntityService.AddAsync(entity);

            return result;
        }

        private DocumentAnalysis MapRequestToDocumentAnalysisEntity(DocumentAnalysisResultViewModel result)
        {
            return new DocumentAnalysis
            {
                IsValid = result.IsValid,
                ValidationErrors = result.ValidationErrors,
                AnalysisDetails = new DocumentAnalysisDetails()
                {
                    FaceMatchScore = result.AnalysisDetails?.FaceMatchScore,
                    DocumentText = result.AnalysisDetails?.DocumentText
                }
            };
        }

    }
}