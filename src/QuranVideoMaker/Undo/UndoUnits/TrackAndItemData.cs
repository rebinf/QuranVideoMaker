using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents a data structure that holds a timeline track and a track item.
    /// </summary>
    public class TrackAndItemData
    {
        /// <summary>
        /// Gets or sets the timeline track.
        /// </summary>
        public ITimelineTrack Track { get; set; }

        /// <summary>
        /// Gets or sets the track item.
        /// </summary>
        public ITrackItem Item { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackAndItemData"/> class.
        /// </summary>
        public TrackAndItemData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackAndItemData"/> class with the specified track and item.
        /// </summary>
        /// <param name="track">The timeline track.</param>
        /// <param name="item">The track item.</param>
        public TrackAndItemData(ITimelineTrack track, ITrackItem item)
        {
            Track = track;
            Item = item;
        }
    }
}
