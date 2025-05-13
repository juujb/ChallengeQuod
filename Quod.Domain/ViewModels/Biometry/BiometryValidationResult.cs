namespace Quod.Domain
{
    public class BiometryValidationResult
    {
        public bool IsValid { get; set; }
        public Fraud Fraud { get; set; } = new Fraud();
        public BiometryType BiometryType { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public ImageMetadataViewModel? Metadata { get; set; }
        public double QualityScore { get; set; }
        public double SimilarityScore { get; set; }
    }

    public class Fraud
    {
        public bool IsFraud { get; set; } = false;
        public string? FraudReason { get; set; }
    }
}
