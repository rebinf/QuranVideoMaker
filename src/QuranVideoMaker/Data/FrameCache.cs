namespace QuranVideoMaker.Data
{
    public class FrameCache
    {
        public int Frame { get; set; }

        public byte[] Data { get; set; }

        public FrameCache(int frame, byte[] data)
        {
            Frame = frame;
            Data = data;
        }
    }
}