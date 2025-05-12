using Microsoft.Extensions.Options;
using Quod.Domain;

namespace Quod.Service
{
    public class ImageValidator : IImageValidator
    {
        private readonly BiometryValidationOptions _options;

        public ImageValidator(IOptions<BiometryValidationOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<ImageValidationResult> ValidateImageAsync(byte[] imageData, ImageType imageType)
        {
            var result = new ImageValidationResult
            {
                IsValid = true,
                SizeInBytes = imageData.Length
            };

            long maxSizeBytes = GetMaxFileSize(imageType);
            if (imageData.Length > maxSizeBytes)
            {
                result.IsValid = false;
                result.Errors.Add($"Tamanho da imagem excede o limite de {maxSizeBytes / 1024} KB");
            }

            var format = DetermineImageFormat(imageData);
            result.Format = format;

            if (format == ImageFormat.Unknown)
            {
                result.IsValid = false;
                result.Errors.Add("Formato de imagem não suportado");
                return result;
            }

            if (!IsFormatAllowed(format, imageType))
            {
                result.IsValid = false;
                result.Errors.Add($"Formato {format} não é permitido para imagens do tipo {imageType}");
            }

            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var image = await SixLabors.ImageSharp.Image.LoadAsync(ms);

                    result.Width = image.Width;
                    result.Height = image.Height;

                    var (minWidth, minHeight) = GetMinimumDimensions(imageType);

                    if (image.Width < minWidth || image.Height < minHeight)
                    {
                        result.IsValid = false;
                        result.Errors.Add($"As dimensões da imagem ({image.Width}x{image.Height}) são menores que o mínimo requerido ({minWidth}x{minHeight})");
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Falha ao processar imagem: {ex.Message}");
            }

            return result;
        }

        private static ImageFormat DetermineImageFormat(byte[] data)
        {
            if (data.Length >= 2)
            {
                // JPEG: FF D8
                if (data[0] == 0xFF && data[1] == 0xD8)
                    return ImageFormat.Jpeg;

                // PNG: 89 50 4E 47 0D 0A 1A 0A
                if (data.Length >= 8 && data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47 &&
                    data[4] == 0x0D && data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A)
                    return ImageFormat.Png;

                // BMP: 42 4D
                if (data[0] == 0x42 && data[1] == 0x4D)
                    return ImageFormat.Bmp;

                // TIFF: 49 49 2A 00 ou 4D 4D 00 2A
                if (data.Length >= 4 &&
                    ((data[0] == 0x49 && data[1] == 0x49 && data[2] == 0x2A && data[3] == 0x00) ||
                     (data[0] == 0x4D && data[1] == 0x4D && data[2] == 0x00 && data[3] == 0x2A)))
                    return ImageFormat.Tiff;

                // WebP: 52 49 46 46 xx xx xx xx 57 45 42 50
                if (data.Length >= 12 && data[0] == 0x52 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x46 &&
                    data[8] == 0x57 && data[9] == 0x45 && data[10] == 0x42 && data[11] == 0x50)
                    return ImageFormat.WebP;
            }

            return ImageFormat.Unknown;
        }

        private long GetMaxFileSize(ImageType imageType)
        {
            return imageType switch
            {
                ImageType.Facial => _options.FacialImage.MaxFileSizeKB * 1024,
                ImageType.Fingerprint => _options.FingerprintImage.MaxFileSizeKB * 1024,
                _ => _options.DefaultMaxFileSizeKB * 1024
            };
        }

        private (int minWidth, int minHeight) GetMinimumDimensions(ImageType imageType)
        {
            return imageType switch
            {
                ImageType.Facial => (_options.FacialImage.MinWidth, _options.FacialImage.MinHeight),
                ImageType.Fingerprint => (_options.FingerprintImage.MinWidth, _options.FingerprintImage.MinHeight),
                _ => (_options.DefaultMinWidth, _options.DefaultMinHeight)
            };
        }

        private bool IsFormatAllowed(ImageFormat format, ImageType imageType)
        {
            var allowedFormats = imageType switch
            {
                ImageType.Facial => _options.FacialImage.AllowedFormats,
                ImageType.Fingerprint => _options.FingerprintImage.AllowedFormats,
                _ => _options.DefaultAllowedFormats
            };

            return allowedFormats.Contains(format);
        }
    }
}