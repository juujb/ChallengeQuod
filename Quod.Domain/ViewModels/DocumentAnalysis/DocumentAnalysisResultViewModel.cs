namespace Quod.Domain
{
    public class DocumentAnalysisResultViewModel
    {
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public DocumentAnalysisDetailsViewModel? AnalysisDetails { get; set; }
    }

    public class DocumentAnalysisDetailsViewModel
    {
        public double? FaceMatchScore { get; set; }
        public string DocumentText { get; set; }  
    }
}
