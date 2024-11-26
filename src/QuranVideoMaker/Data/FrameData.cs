using SkiaSharp;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents the data for a single frame in a video.
    /// </summary>
    public class FrameData
    {
        /// <summary>
        /// Gets or sets the opacity of the frame.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the raw data of the frame.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the bitmap of the frame.
        /// </summary>
        public SKBitmap Bitmap { get; set; }

        /// <summary>
        /// Gets or sets the order of the frame.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameData"/> class with raw data, opacity, and order.
        /// </summary>
        /// <param name="data">The raw data of the frame.</param>
        /// <param name="opacity">The opacity of the frame.</param>
        /// <param name="order">The order of the frame.</param>
        public FrameData(byte[] data, double opacity, int order)
        {
            Data = data;
            Opacity = opacity;
            Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameData"/> class with a bitmap, opacity, and order.
        /// </summary>
        /// <param name="bitmap">The bitmap of the frame.</param>
        /// <param name="opacity">The opacity of the frame.</param>
        /// <param name="order">The order of the frame.</param>
        public FrameData(SKBitmap bitmap, double opacity, int order)
        {
            Bitmap = bitmap;
            Opacity = opacity;
            Order = order;
        }
    }
}