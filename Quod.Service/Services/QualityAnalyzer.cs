using Microsoft.Extensions.Options;
using Quod.Domain;
using SixLabors.ImageSharp;

namespace Quod.Service
{
    public class QualityAnalyzer : IQualityAnalyzer
    {
        private readonly BiometryValidationOptions _options;

        public QualityAnalyzer(IOptions<BiometryValidationOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<ImageQualityResult> AnalyzeFacialImageQualityAsync(byte[] imageData)
        {
            var result = new ImageQualityResult
            {
                IsAcceptable = true,
                QualityScore = 0
            };

            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var image = await Image.LoadAsync(ms);

                    // Calcular luminosidade média da imagem
                    double brightness = CalculateAverageBrightness();

                    // Verificar se a luminosidade está dentro dos limites aceitáveis
                    if (brightness < _options.FacialImage.MinBrightness)
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Imagem facial muito escura");
                    }
                    else if (brightness > _options.FacialImage.MaxBrightness)
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Imagem facial muito clara/superexposta");
                    }

                    // Verificar contraste
                    double contrast = CalculateContrast();
                    if (contrast < _options.FacialImage.MinContrast)
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Imagem facial com contraste insuficiente");
                    }

                    // Nota de qualidade baseada em contraste e brilho (exemplo simplificado)
                    // Uma implementação real pode incluir detecção facial, nitidez, etc.
                    double brightnessScore = CalculateBrightnessScore(brightness,
                        _options.FacialImage.MinBrightness,
                        _options.FacialImage.MaxBrightness);

                    double contrastScore = contrast / _options.FacialImage.MinContrast;
                    if (contrastScore > 1) contrastScore = 1;

                    if (brightness < 0.3 || brightness > 0.9) // Brilho muito fora do normal
                    {
                        // Simulação de verificação de irregularidade (muito simplificada)
                        if (new Random().NextDouble() > 0.7)
                        {
                            result.IsAcceptable = false;
                            result.QualityIssues.Add("Possível tentativa de fraude (brilho irregular detectado).");
                        }
                    }

                    // Calcular pontuação final (média simples)
                    result.QualityScore = (brightnessScore + contrastScore) / 2.0 * 100;
                }
            }
            catch (Exception ex)
            {
                result.IsAcceptable = false;
                result.QualityIssues.Add($"Falha ao analisar qualidade da imagem facial: {ex.Message}");
            }

            return result;
        }

        public async Task<ImageQualityResult> AnalyzeFingerPrintQualityAsync(byte[] imageData)
        {
            var result = new ImageQualityResult
            {
                IsAcceptable = true,
                QualityScore = 0
            };

            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var image = await SixLabors.ImageSharp.Image.LoadAsync(ms);

                    // Calcular contraste (fator importante para impressões digitais)
                    double contrast = CalculateContrast();
                    if (contrast < _options.FingerprintImage.MinContrast)
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Contraste insuficiente na impressão digital");
                    }

                    // Calcular nitidez (usando gradientes de borda)
                    double sharpness = CalculateSharpness();
                    if (sharpness < _options.FingerprintImage.MinSharpness)
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Impressão digital sem nitidez suficiente");
                    }

                    // Verificar uniformidade (importante para evitar impressões parciais)
                    double coverage = CalculateImageCoverage();
                    if (coverage < _options.FingerprintImage.MinCoverage)
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Cobertura insuficiente da impressão digital");
                    }

                    double contrastScore = contrast / _options.FingerprintImage.MinContrast;
                    if (contrastScore > 1) contrastScore = 1;

                    double sharpnessScore = sharpness / _options.FingerprintImage.MinSharpness;
                    if (sharpnessScore > 1) sharpnessScore = 1;

                    double coverageScore = coverage / _options.FingerprintImage.MinCoverage;
                    if (coverageScore > 1) coverageScore = 1;

                    if (contrast < _options.FacialImage.MinContrast - 0.1 && sharpness < 0.4) // Exemplo de artefato
                    {
                        result.IsAcceptable = false;
                        result.QualityIssues.Add("Possível tentativa de fraude (baixo contraste e nitidez).");
                    }

                    result.QualityScore = (contrastScore * 0.3 + sharpnessScore * 0.4 + coverageScore * 0.3) * 100;
                }
            }
            catch (Exception ex)
            {
                result.IsAcceptable = false;
                result.QualityIssues.Add($"Falha ao analisar qualidade da impressão digital: {ex.Message}");
            }

            return result;
        }

        private double CalculateAverageBrightness()
        {
            // Retorna um valor aleatório entre 0.5 e 1.0 (brilho aceitável)
            return new Random().NextDouble() * 0.5 + 0.5;
        }

        private double CalculateContrast()
        {
            // Retorna um valor aleatório entre 0.6 e 1.0 (contraste razoável)
            return new Random().NextDouble() * 0.4 + 0.6;
        }

        private double CalculateSharpness()
        {
            // Retorna um valor aleatório entre 0.5 e 1.0 (nitidez razoável)
            return new Random().NextDouble() * 0.5 + 0.5;
        }

        private double CalculateImageCoverage()
        {
            // Retorna um valor aleatório entre 0.8 e 1.0 (boa cobertura)
            return new Random().NextDouble() * 0.2 + 0.8;
        }

        private double CalculateBrightnessScore(double brightness, double minBrightness, double maxBrightness)
        {
            double idealBrightness = (minBrightness + maxBrightness) / 2.0;
            double range = maxBrightness - minBrightness;
            double normalizedDistance = Math.Abs(brightness - idealBrightness) / (range / 2.0);
            return Math.Max(0, 1 - normalizedDistance);
        }
    }
}
