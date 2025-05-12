using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Quod.Domain
{
    public class DocumentAnalysisRequestViewModel
    {
        public required IFormFile DocumentImageFile { get; set; }

        public required IFormFile FaceImageFile { get; set; }

        [JsonIgnore]
        public byte[]? DocumentImage { get; set; }

        [JsonIgnore]
        public byte[]? FaceImage { get; set; }
    }
}