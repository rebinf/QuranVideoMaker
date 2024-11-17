using System.Collections.ObjectModel;
using System.ComponentModel;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents a timeline track that contains different types of track items.
    /// </summary>
    public interface ITimelineTrack : INotifyPropertyChanged
    {
        /// <summary>
        /// The Id of the timeline track.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The name of the timeline track.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The type of the timeline track.
        /// </summary>
        TimelineTrackType Type { get; set; }

        /// <summary>
        /// The height of the timeline track.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the collection of track items in this track.
        /// </summary>
        ObservableCollection<ITrackItem> Items { get; set; }

        /// <summary>
        /// Gets the duration of the track, calculated by the longest track item.
        /// </summary>
        TimeCode GetDuration();

        /// <summary>
        /// Cuts the item at the specified timeline frame.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="timelineFrame">The timeline frame.</param>
        void CutItem(ITrackItem item, double timelineFrame);

        /// <summary>
        /// Gets the track item at the specified timeline frame.
        /// </summary>
        ITrackItem GetItemAtTimelineFrame(double timelineFrame);

    }
}