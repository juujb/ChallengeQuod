using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Quod.Domain
{
    public class DocumentAnalysisRequestViewModel
    {
        public required IFormFile DocumentImageFile { get; set; }

        public required IFormFile FaceImageFile { get; set; }

        [SwaggerIgnore]
        public byte[]? DocumentImage { get; set; }

        [SwaggerIgnore]
        public byte[]? FaceImage { get; set; }
    }
}