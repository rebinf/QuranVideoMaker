using System.Diagnostics;

namespace QuranVideoMaker.Data
{
    [DebuggerDisplay("Frame: {Frame}")]
    public class FrameContainer
    {
        public int Frame { get; set; }

        public byte[] Data { get; set; }

        public FrameContainer(int frame)
        {
            Frame = frame;
        }
    }
}