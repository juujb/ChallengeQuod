using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quod.Domain
{
    public interface IQualityAnalyzer
    {
        Task<ImageQualityResult> AnalyzeFacialImageQualityAsync(byte[] imageData);
        Task<ImageQualityResult> AnalyzeFingerPrintQualityAsync(byte[] imageData);
    }
}
