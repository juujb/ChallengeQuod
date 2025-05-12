using Quod.Domain;

namespace Quod.Service
{
    public class DocumentAnalysisService : IDocumentAnalysisService
    {
        private readonly IImageValidator _imageValidator;

        public DocumentAnalysisService(IImageValidator imageValidator)
        {
            _imageValidator = imageValidator ?? throw new ArgumentNullException(nameof(imageValidator));
        }

        public async Task<DocumentAnalysisResultViewModel> AnalyzeDocumentAsync(DocumentAnalysisRequestViewModel request)
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

            double faceMatchScore = new Random().NextDouble() * 0.8 + 0.2;
            if (faceMatchScore < 0.5)
            {
                result.IsValid = false;
                result.ValidationErrors.Add($"A pontuação de similaridade da face é baixa: {faceMatchScore:P}");
            }

            bool documentStructureValid = new Random().Next(0, 2) == 1;
            bool fontsConsistent = new Random().Next(0, 2) == 1;

            if (!documentStructureValid)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("A estrutura do documento parece inválida.");
            }

            if (!fontsConsistent)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("As fontes no documento parecem inconsistentes.");
            }

            if (result.IsValid)
            {
                result.AnalysisDetails = new DocumentAnalysisDetails
                {
                    FaceMatchScore = faceMatchScore,
                    DocumentStructureValid = documentStructureValid,
                    FontsConsistent = fontsConsistent
                };
            }

            return result;
        }

    }
}