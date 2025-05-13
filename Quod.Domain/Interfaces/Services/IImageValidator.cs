namespace Quod.Domain
{
    public interface IImageValidator
    {
        Task<ImageValidationResult> ValidateImageAsync(byte[] imageData, ImageType imageType);
    }
}
