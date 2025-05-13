namespace Quod.Domain
{
    public interface IImageCompareService
    {
        (bool, double) IsImageSimilar(byte[] imageBytes1, byte[] imageBytes2, double threshold = 0.5);
    }
}
