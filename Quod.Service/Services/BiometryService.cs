using Quod.Domain;

namespace Quod.Service
{
    public class BiometryService : IBiometryService
    {
        private readonly IImageValidator _imageValidator;
        private readonly IMetadataExtractor _metadataExtractor;
        private readonly IQualityAnalyzer _qualityAnalyzer;

        public BiometryService(
            IImageValidator imageValidator,
            IMetadataExtractor metadataExtractor,
            IQualityAnalyzer qualityAnalyzer)
        {
            _imageValidator = imageValidator ?? throw new ArgumentNullException(nameof(imageValidator));
            _metadataExtractor = metadataExtractor ?? throw new ArgumentNullException(nameof(metadataExtractor));
            _qualityAnalyzer = qualityAnalyzer ?? throw new ArgumentNullException(nameof(qualityAnalyzer));
        }

        public async Task<BiometryValidationResult> CheckFacialBiometryAsync(BiometryRequestViewModel request)
        {
            return await ProcessBiometryAsync(request, BiometryType.Facial);
        }

        public async Task<BiometryValidationResult> CheckFingerPrintBiometryAsync(BiometryRequestViewModel request)
        {
            return await ProcessBiometryAsync(request, BiometryType.Fingerprint);
        }

        private async Task<BiometryValidationResult> ProcessBiometryAsync(
            BiometryRequestViewModel request,
            BiometryType biometryType)
        {
            request.ImageFile = await request.Image.ConvertToByteArrayAsync();

            ValidateRequest(request);

            var result = new BiometryValidationResult
            {
                BiometryType = biometryType,
                IsValid = true
            };

            try
            {
                var imageType = GetImageType(biometryType);
                var basicValidation = await _imageValidator.ValidateImageAsync(request.BiometryData, imageType);
                if (!basicValidation.IsValid)
                {
                    result.IsValid = false;
                    result.ValidationErrors.AddRange(basicValidation.Errors);
                    return result;
                }

                var metadata = await _metadataExtractor.ExtractMetadataAsync(request.BiometryData);
                result.Metadata = metadata;

                if (biometryType == BiometryType.Facial && request.DeviceInfo != null && metadata?.DeviceInfo != null &&
                    !string.IsNullOrEmpty(metadata.DeviceInfo.Manufacturer) &&
                    !string.IsNullOrEmpty(request.DeviceInfo.Manufacturer) &&
                    !metadata.DeviceInfo.Manufacturer.Equals(request.DeviceInfo.Manufacturer, StringComparison.OrdinalIgnoreCase))
                {
                    result.IsValid = false;
                    result.ValidationErrors.Add("O fabricante do dispositivo nos metadados não corresponde ao informado");
                }

                ImageQualityResult? qualityResult = null;

                if (biometryType == BiometryType.Facial)
                {
                    qualityResult = await _qualityAnalyzer.AnalyzeFacialImageQualityAsync(request.BiometryData);
                }
                else if (biometryType == BiometryType.Fingerprint)
                {
                    qualityResult = await _qualityAnalyzer.AnalyzeFingerPrintQualityAsync(request.BiometryData);
                }

                if (qualityResult != null && !qualityResult.IsAcceptable)
                {
                    result.IsValid = false;
                    result.ValidationErrors.AddRange(qualityResult.QualityIssues);
                }

                result.QualityScore = qualityResult?.QualityScore ?? 0;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ValidationErrors.Add($"Erro ao processar imagem: {ex.Message}");
            }

            return result;
        }

        private ImageType GetImageType(BiometryType biometryType)
        {
            return biometryType switch
            {
                BiometryType.Facial => ImageType.Facial,
                BiometryType.Fingerprint => ImageType.Fingerprint,
                _ => throw new ArgumentOutOfRangeException(nameof(biometryType), biometryType, "Tipo de biometria não suportado")
            };
        }

        private void ValidateRequest(BiometryRequestViewModel request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.BiometryData == null || request.BiometryData.Length == 0)
                throw new BiometryException("Os dados biométricos não podem estar vazios");

            if (request.DeviceInfo == null)
                throw new BiometryException("As informações do dispositivo são obrigatórias");
        }
    }
}