using OpenCvSharp;
using SkiaSharp;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Data
{
    public class FrameCache
    {
        public int Frame { get; set; }

        public byte[] Data { get; set; }

        //[JsonIgnore]
        //public SKBitmap Bitmap { get; set; }

        //public FrameCache(int frame, SKBitmap bitmap)
        //{
        //    Frame = frame;
        //    Bitmap = bitmap;
        //}

        public FrameCache(int frame, byte[] data)
        {
            Frame = frame;
            Data = data;
        }
    }
}