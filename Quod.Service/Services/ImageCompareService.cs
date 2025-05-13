using OpenCvSharp;
using Quod.Domain;

namespace Quod.Service
{
    public class ImageCompareService : IImageCompareService
    {

        public (bool, double) IsImageSimilar(byte[] imageBytes1, byte[] imageBytes2, double threshold = 0.5)
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

            Cv2.Normalize(histogram1, histogram1, 0, 1, NormTypes.MinMax);
            Cv2.Normalize(histogram2, histogram2, 0, 1, NormTypes.MinMax);

            double score = Cv2.CompareHist(histogram1, histogram2, HistCompMethods.Bhattacharyya);

            return score;
        }

        private static Mat GetHistogramGrayScale(Mat imagem, int numeroDeBins)
        {
            Rangef intervalos = new ( 0, 256 );
            Mat histograma = new ();
            Cv2.CalcHist([imagem], [1], null, histograma, 1,  [ numeroDeBins ], [intervalos]);
            return histograma;
        }

    }
}
