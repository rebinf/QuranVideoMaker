using OpenCvSharp;

namespace QuranVideoMaker.Data
{
    public class FrameContainer
    {
        public int Frame { get; set; }

        public Mat Data { get; set; }

        public double Opacity { get; set; }

        public byte[] Rendered { get; set; }

        public FrameContainer(int frame)
        {
            Frame = frame;
        }
    }
}