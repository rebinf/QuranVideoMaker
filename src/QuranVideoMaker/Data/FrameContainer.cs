using System.Diagnostics;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents a container for a single frame of video data.
    /// </summary>
    [DebuggerDisplay("Frame: {Frame}")]
    public class FrameContainer
    {
        /// <summary>
        /// Gets or sets the frame number.
        /// </summary>
        public int FrameNumber { get; set; }

        /// <summary>
        /// Gets or sets the data for the frame.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameContainer"/> class with the specified frame number.
        /// </summary>
        /// <param name="frame">The frame number.</param>
        public FrameContainer(int frame)
        {
            FrameNumber = frame;
        }
    }
}