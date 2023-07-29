using FFMpegCore.Pipes;
using System.IO;

namespace QuranVideoMaker.Utilities
{
    public class FramePipeSource : IPipeSource
    {
        public double FPS { get; set; }

        private readonly IEnumerator<byte[]> _frames;

        public FramePipeSource(IEnumerator<byte[]> frames, double fps = 25)
        {
            FPS = fps;
            _frames = frames;
        }

        public FramePipeSource(IEnumerable<byte[]> frames, double fps = 25) : this(frames.GetEnumerator(), fps) { }

        public string GetStreamArguments()
        {
            return $"-f image2pipe -framerate {FPS} ";
        }

        public async Task WriteAsync(Stream outputStream, CancellationToken cancellationToken)
        {
            if (_frames.Current != null)
            {
                await outputStream.WriteAsync(_frames.Current, 0, _frames.Current.Length, cancellationToken);
            }

            while (_frames.MoveNext())
            {
                await outputStream.WriteAsync(_frames!.Current, 0, _frames.Current!.Length, cancellationToken);
            }
        }
    }
}
