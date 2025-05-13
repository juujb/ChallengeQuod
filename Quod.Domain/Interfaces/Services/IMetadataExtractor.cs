namespace Quod.Domain
{
    public interface IMetadataExtractor
    {
        Task<ImageMetadataViewModel> ExtractMetadataAsync(byte[] imageData);
    }
}