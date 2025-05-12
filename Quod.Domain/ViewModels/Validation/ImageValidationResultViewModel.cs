namespace Quod.Domain
{
    public class ImageValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public int Width { get; set; }
        public int Height { get; set; }
        public long SizeInBytes { get; set; }
        public ImageFormat Format { get; set; }
    }
}
