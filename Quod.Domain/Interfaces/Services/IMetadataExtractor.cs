namespace Quod.Domain
{
    public interface IMetadataExtractor
    {
        Task<ImageMetadata> ExtractMetadataAsync(byte[] imageData);
    }
}