namespace Quod.Domain
{
    public class ImageQualityResult
    {
        public bool IsAcceptable { get; set; }
        public double QualityScore { get; set; }
        public List<string> QualityIssues { get; set; } = new List<string>();
    }
}
