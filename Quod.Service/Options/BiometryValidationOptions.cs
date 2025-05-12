using Quod.Domain;

namespace Quod.Service
{
    /// <summary>
    /// Configurações de validação para imagens biométricas
    /// </summary>
    public class BiometryValidationOptions
    {
        /// <summary>
        /// Configurações para imagens faciais
        /// </summary>
        public FacialImageOptions FacialImage { get; set; } = new FacialImageOptions();

        /// <summary>
        /// Configurações para impressões digitais
        /// </summary>
        public FingerprintImageOptions FingerprintImage { get; set; } = new FingerprintImageOptions();

        /// <summary>
        /// Tamanho máximo padrão em KB para imagens biométricas
        /// </summary>
        public int DefaultMaxFileSizeKB { get; set; } = 5120; // 5MB

        /// <summary>
        /// Largura mínima padrão para imagens biométricas
        /// </summary>
        public int DefaultMinWidth { get; set; } = 480;

        /// <summary>
        /// Altura mínima padrão para imagens biométricas
        /// </summary>
        public int DefaultMinHeight { get; set; } = 480;

        /// <summary>
        /// Formatos de arquivo permitidos por padrão
        /// </summary>
        public List<ImageFormat> DefaultAllowedFormats { get; set; } =
        [
            ImageFormat.Jpeg,
            ImageFormat.Png
        ];
    }

    /// <summary>
    /// Configurações específicas para imagens faciais
    /// </summary>
    public class FacialImageOptions
    {
        /// <summary>
        /// Tamanho máximo do arquivo em KB
        /// </summary>
        public int MaxFileSizeKB { get; set; } = 10240; // 10MB

        /// <summary>
        /// Largura mínima em pixels
        /// </summary>
        public int MinWidth { get; set; } = 640;

        /// <summary>
        /// Altura mínima em pixels
        /// </summary>
        public int MinHeight { get; set; } = 480;

        /// <summary>
        /// Brilho mínimo aceitável (0-1)
        /// </summary>
        public double MinBrightness { get; set; } = 0.3;

        /// <summary>
        /// Brilho máximo aceitável (0-1)
        /// </summary>
        public double MaxBrightness { get; set; } = 0.8;

        /// <summary>
        /// Contraste mínimo aceitável (0-1)
        /// </summary>
        public double MinContrast { get; set; } = 0.4;

        /// <summary>
        /// Formatos de arquivo permitidos
        /// </summary>
        public List<ImageFormat> AllowedFormats { get; set; } =
        [
            ImageFormat.Jpeg,
            ImageFormat.Png
        ];
    }

    /// <summary>
    /// Configurações específicas para imagens de impressão digital
    /// </summary>
    public class FingerprintImageOptions
    {
        /// <summary>
        /// Tamanho máximo do arquivo em KB
        /// </summary>
        public int MaxFileSizeKB { get; set; } = 5120; // 5MB

        /// <summary>
        /// Largura mínima em pixels
        /// </summary>
        public int MinWidth { get; set; } = 500;

        /// <summary>
        /// Altura mínima em pixels
        /// </summary>
        public int MinHeight { get; set; } = 500;

        /// <summary>
        /// Contraste mínimo aceitável (0-1)
        /// </summary>
        public double MinContrast { get; set; } = 0.5;

        /// <summary>
        /// Nitidez mínima aceitável (0-1)
        /// </summary>
        public double MinSharpness { get; set; } = 0.6;

        /// <summary>
        /// Cobertura mínima aceitável (0-1)
        /// </summary>
        public double MinCoverage { get; set; } = 0.7;

        /// <summary>
        /// Formatos de arquivo permitidos
        /// </summary>
        public List<ImageFormat> AllowedFormats { get; set; } =
        [
            ImageFormat.Jpeg,
            ImageFormat.Png,
            ImageFormat.Tiff
        ];
    }
}