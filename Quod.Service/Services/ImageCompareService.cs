using OpenCvSharp;
using Quod.Domain;

namespace Quod.Service
{
    public class ImageCompareService : IImageCompareService
    {

        public (bool, double) IsImageSimilar(byte[] imageBytes1, byte[] imageBytes2, double threshold = 0.4)
        {
            double score = GetCompareScore(imageBytes1, imageBytes2);
            return (score < threshold, score);
        }

        private static double GetCompareScore(byte[] imageBytes1, byte[] imageBytes2, int bins = 256)
        {
            using var imagem1 = Cv2.ImDecode(imageBytes1, ImreadModes.Grayscale);
            using var imagem2 = Cv2.ImDecode(imageBytes2, ImreadModes.Grayscale);

            if (imagem1.Empty() || imagem2.Empty())
                return 1;

            using var histogram1 = GetHistogramGrayScale(imagem1, bins);
            using var histogram2 = GetHistogramGrayScale(imagem2, bins);

            double score = Cv2.CompareHist(histogram1, histogram2, HistCompMethods.Bhattacharyya);

            return score;
        }

        private static Mat GetHistogramGrayScale(Mat imagem, int numeroDeBins)
        {
            Rangef range = new(0, 256);
            int[] histSize = new int[] { 256 };
            int[] dim = new int[] { 0 };
            Mat histogram = new ();
            Cv2.CalcHist([imagem], [0], null, histogram, 1, histSize, [range]);
            return histogram;
        }

    }
}
