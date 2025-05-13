using Quod.Domain;

namespace Quod.Service
{
    public class BiometryService : IBiometryService
    {
        private readonly IImageValidator _imageValidator;
        private readonly IMetadataExtractor _metadataExtractor;
        private readonly IQualityAnalyzer _qualityAnalyzer;
        private readonly IImageCompareService _imageCompareService;
        private readonly IBiometryEntityService _biometryEntityService;
        private readonly INotificationRestService _notificationRestService;

        public BiometryService(
            IImageValidator imageValidator,
            IMetadataExtractor metadataExtractor,
            IQualityAnalyzer qualityAnalyzer,
            IImageCompareService imageCompareService,
            IBiometryEntityService biometryEntityService,
            INotificationRestService notificationRestService)
        {
            _imageValidator = imageValidator ?? throw new ArgumentNullException(nameof(imageValidator));
            _metadataExtractor = metadataExtractor ?? throw new ArgumentNullException(nameof(metadataExtractor));
            _qualityAnalyzer = qualityAnalyzer ?? throw new ArgumentNullException(nameof(qualityAnalyzer));
            _imageCompareService = imageCompareService ?? throw new ArgumentNullException(nameof(imageCompareService));
            _biometryEntityService = biometryEntityService ?? throw new ArgumentNullException(nameof(biometryEntityService));
            _notificationRestService = notificationRestService ?? throw new ArgumentNullException(nameof(notificationRestService));
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
            request.ImageBytes = await request.Image.ConvertToByteArrayAsync();
            request.ExpectedBytes = await request.ExpectedBiometry.ConvertToByteArrayAsync();

            ValidateRequest(request);

            var result = new BiometryValidationResult
            {
                BiometryType = biometryType,
                IsValid = true
            };

            try
            {
                var imageType = GetImageType(biometryType);
                var basicValidation = await _imageValidator.ValidateImageAsync(request.ImageBytes, imageType);
                if (!basicValidation.IsValid)
                {
                    result.IsValid = false;
                    result.ValidationErrors.AddRange(basicValidation.Errors);
                    return result;
                }

                var metadata = await _metadataExtractor.ExtractMetadataAsync(request.ImageBytes);
                result.Metadata = metadata;

                if (biometryType == BiometryType.Facial && request.DeviceInfo != null && metadata?.DeviceInfo != null &&
                    !string.IsNullOrEmpty(metadata.DeviceInfo.Manufacturer) &&
                    !string.IsNullOrEmpty(request.DeviceInfo.Manufacturer) &&
                    !metadata.DeviceInfo.Manufacturer.Equals(request.DeviceInfo.Manufacturer, StringComparison.OrdinalIgnoreCase))
                {
                    result.IsValid = false;
                    result.Fraud.IsFraud = true;
                    result.Fraud.FraudReason = "Fabricante do dispositivo não corresponde ao informado";
                    result.ValidationErrors.Add("O fabricante do dispositivo nos metadados não corresponde ao informado");
                }

                ImageQualityResult? qualityResult = null;

                qualityResult = await GetImageQualityResultAsync(request.ImageBytes, biometryType);

                if (qualityResult != null && !qualityResult.IsAcceptable)
                {
                    result.IsValid = false;
                    result.ValidationErrors.AddRange(qualityResult.QualityIssues);
                }

                var (isBiometrySimilar, similarityScore) = _imageCompareService.IsImageSimilar(request.ImageBytes, request.ExpectedBytes);
                result.SimilarityScore = similarityScore;

                if (!isBiometrySimilar)
                {
                    result.IsValid = false;
                    result.Fraud.IsFraud = true;
                    result.Fraud.FraudReason = "A imagem não é semelhante à biometria fornecida";
                    result.ValidationErrors.Add("A imagem não é semelhante à biometria fornecida");
                }

                result.QualityScore = qualityResult?.QualityScore ?? 0;

                var entity = MapRequestToBiometryEntity(request, result);
                var notification = MapRequestToNotification(request, result);

                List<Task> tasks = new List<Task>(2);

                tasks.Add(_biometryEntityService.AddAsync(entity));

                if (result.Fraud.IsFraud) tasks.Add(_notificationRestService.PostAsync(notification));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ValidationErrors.Add($"Erro ao processar imagem: {ex.Message}");
            }

            return result;
        }

        private static NotificationViewModel MapRequestToNotification(BiometryRequestViewModel request, BiometryValidationResult result)
        {
            return new NotificationViewModel
            {
                TransactionId = Guid.NewGuid(),
                BiometryType = result.BiometryType.ToString(),
                FraudType = result.Fraud.FraudReason!,
                CaptureDate = request.CaptureDate,
                Device = new DeviceViewModel
                {
                    Manufacturer = request.DeviceInfo.Manufacturer,
                    Model = request.DeviceInfo.Model,
                    OperatingSystem = request.DeviceInfo.OS
                },
                NotificationChannels = new List<string> { "email", "sms" },
                NotifiedBy = "biometry-service",
                Metadata = result.Metadata != null ? new MetadataViewModel
                {
                    Latitude = result.Metadata.Location?.Latitude ?? 0,
                    Longitude = result.Metadata.Location?.Longitude ?? 0,
                    SourceIp = result.Metadata.RawMetadata.ContainsKey("ipOrigem")
                        ? result.Metadata.RawMetadata["ipOrigem"]
                        : string.Empty
                } : null
            };
        }

        private static Biometry MapRequestToBiometryEntity(BiometryRequestViewModel request, BiometryValidationResult result)
        {
            return new Biometry
            {
                BiometryType = result.BiometryType,
                CaptureDate = request.CaptureDate,
                DeviceInfo = new Device
                {
                    Manufacturer = request.DeviceInfo.Manufacturer,
                    Model = request.DeviceInfo.Model,
                    OperatingSystem = request.DeviceInfo.OS
                },
                Metadata = result.Metadata != null ? new Metadata
                {
                    Latitude = result.Metadata.Location?.Latitude ?? 0,
                    Longitude = result.Metadata.Location?.Longitude ?? 0,
                    SourceIp = result.Metadata.RawMetadata.ContainsKey("ipOrigem")
                        ? result.Metadata.RawMetadata["ipOrigem"]
                        : string.Empty
                } : null,
                QualityScore = result.QualityScore,
                SimilarityScore = result.SimilarityScore,
                ValidationErrors = result.ValidationErrors,
                IsValid = result.IsValid
            };
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

        private async Task<ImageQualityResult> GetImageQualityResultAsync(byte[] biometryData, BiometryType biometryType)
        {
            return biometryType switch
            {
                BiometryType.Facial => await _qualityAnalyzer.AnalyzeFacialImageQualityAsync(biometryData),
                BiometryType.Fingerprint => await _qualityAnalyzer.AnalyzeFingerPrintQualityAsync(biometryData),
                _ => throw new ArgumentOutOfRangeException(nameof(biometryType), biometryType, "Tipo de biometria não suportado")
            };
        }

        private void ValidateRequest(BiometryRequestViewModel request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.ImageBytes == null || request.ImageBytes.Length == 0)
                throw new BiometryException("Os dados biométricos não podem estar vazios");

            if (request.DeviceInfo == null)
                throw new BiometryException("As informações do dispositivo são obrigatórias");
        }
    }
}