using FFMpegCore.Pipes;
using System.IO;

namespace QuranVideoMaker.Utilities
{
	public class FramePipeSource : IPipeSource
	{
		public double FPS { get; set; }

		private readonly IEnumerable<byte[]> _frames;

		public FramePipeSource(IEnumerable<byte[]> frames, double fps = 25)
		{
			FPS = fps;
			_frames = frames;
		}

		public string GetStreamArguments()
		{
			return $"-f image2pipe -framerate {FPS} ";
		}

		public async Task WriteAsync(Stream outputStream, CancellationToken cancellationToken)
		{
			var array = _frames.ToArray();

			for (int i = 0; i < _frames.Count(); i++)
			{
				var current = array[i];
				await outputStream.WriteAsync(current, 0, current.Length, cancellationToken);
			}
		}
	}
}
