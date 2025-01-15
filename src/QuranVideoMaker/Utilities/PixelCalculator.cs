using QuranVideoMaker.Data;

namespace QuranVideoMaker.Utilities
{
    public static class PixelCalculator
    {
        public static double GetPixels(double totalFrames, int zoom)
        {
            var pixels = totalFrames / Constants.TimelineZooms[zoom] * Constants.TimelinePixelsInSeparator;
            return pixels;
        }
    }
}
