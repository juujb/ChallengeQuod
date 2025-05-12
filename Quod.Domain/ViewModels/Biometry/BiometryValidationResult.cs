namespace Quod.Domain
{
    public class BiometryValidationResult
    {
        public bool IsValid { get; set; }
        public BiometryType BiometryType { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public ImageMetadata? Metadata { get; set; }
        public double QualityScore { get; set; }
    }
}
