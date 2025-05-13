namespace Quod.Domain
{
    public interface IQualityAnalyzer
    {
        Task<ImageQualityResult> AnalyzeFacialImageQualityAsync(byte[] imageData);
        Task<ImageQualityResult> AnalyzeFingerPrintQualityAsync(byte[] imageData);
    }
}
