using OpenCvSharp;
using SkiaSharp;

namespace QuranVideoMaker.Data
{
    public class FrameData
    {
        public double Opacity { get; set; }

        public byte[] Data { get; set; }

        public SKBitmap Bitmap { get; set; }

        public int Order { get; set; }

        public FrameData(byte[] data, double opacity, int order)
        {
            Data = data;
            Opacity = opacity;
            Order = order;
        }

        public FrameData(SKBitmap bitmap, double opacity, int order)
        {
            Bitmap = bitmap;
            Opacity = opacity;
            Order = order;
        }
    }
}