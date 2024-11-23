using FFMpegCore.Pipes;
using QuranVideoMaker.Data;
using System.Collections.Concurrent;
using System.IO;

namespace QuranVideoMaker.Utilities
{
    public class MultithreadedFramePipeSource : IPipeSource
    {
        private List<int> _processedFrames = new List<int>();
        private int _lastFrameNumber = 0;
        private int _totalFrames;
        private Lock _lock = new Lock();

        private List<FrameContainer> _frames { get; set; } = new List<FrameContainer>();

        public double FPS { get; set; }

        public bool IsFinished { get; set; }

        public bool IsReady { get; set; }

        public MultithreadedFramePipeSource(double fps = 25, int totalFrames = 0)
        {
            FPS = fps;
            _totalFrames = totalFrames;
        }

        public void AddFrame(FrameContainer frame)
        {
            using (_lock.EnterScope())
            {
                _frames.Add(frame);
            }
        }

        public int GetProcessedFramesCount()
        {
            using (_lock.EnterScope())
            {
                return _processedFrames.Count;
            }
        }

        public int GetUnprocessedFramesCount()
        {
            using (_lock.EnterScope())
            {
                return _frames.Count;
            }
        }

        public string GetStreamArguments()
        {
            return $"-f image2pipe -framerate {FPS} ";
        }

        public async Task WriteAsync(Stream outputStream, CancellationToken cancellationToken)
        {
            IsReady = true;

            while (!IsFinished)
            {
                if (_lastFrameNumber == _totalFrames)
                {
                    IsFinished = true;
                    break;
                }

                var currentFrame = _lastFrameNumber + 1;

                var frame = _frames.FirstOrDefault(x => x.Frame == currentFrame);

                // make sure next frame is the last frame number + 1
                if (frame != null)
                {
                    await outputStream.WriteAsync(frame.Data, 0, frame.Data.Length, cancellationToken);
                    _lastFrameNumber = frame.Frame;

                    _processedFrames.Add(frame.Frame);

                    // remove the frame from the list
                    using (_lock.EnterScope())
                    {
                        _frames.Remove(frame);
                    }

                    // set the frame data to null
                    frame.Data = null;

                    // collect the garbage
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                else
                {
                    await Task.Delay(1);
                }
            }
        }
    }
}
