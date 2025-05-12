namespace Quod.Domain
{
    public class DocumentAnalysisResultViewModel
    {
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public DocumentAnalysisDetails? AnalysisDetails { get; set; }
    }

    public class DocumentAnalysisDetails
    {
        public double? FaceMatchScore { get; set; }
        public bool? DocumentStructureValid { get; set; }
        public bool? FontsConsistent { get; set; }
    }
}
