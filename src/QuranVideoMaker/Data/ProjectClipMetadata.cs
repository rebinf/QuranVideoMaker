namespace QuranVideoMaker.Data
{
    /// <summary>
    /// This class represents the metadata of a project clip.
    /// </summary>
    public class ProjectClipMetadata
    {
        /// <summary>
        /// The type of the timeline track.
        /// </summary>
        public TimelineTrackType TrackType { get; set; }

        /// <summary>
        /// The type of the track item.
        /// </summary>
        public TrackItemType ItemType { get; set; }

        /// <summary>
        /// The length of the clip in timecode format.
        /// </summary>
        public TimeCode Length { get; set; }

        /// <summary>
        /// The frames per second of the clip.
        /// </summary>
        public double FPS { get; set; }

        /// <summary>
        /// The width in pixels of the clip.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height in pixels of the clip.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Indicates whether the clip has unlimited length.
        /// </summary>
        public bool UnlimitedLength { get; set; }
    }
}
