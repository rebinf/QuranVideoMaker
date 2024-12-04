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
        /// Occurs when the track is changed.
        /// </summary>
        event EventHandler Changed;

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
        /// Adds a track item to the track.
        /// </summary>
        /// <param name="item">The track item to add.</param>
        void AddTrackItem(ITrackItem item);

        /// <summary>
        /// Adds a collection of track items to the track.
        /// </summary>
        /// <param name="items">The collection of track items to add.</param>
        void AddTrackItems(IEnumerable<ITrackItem> items);

        /// <summary>
        /// Removes a track item from the track.
        /// </summary>
        /// <param name="item">The track item to remove.</param>
        void RemoveTrackItem(ITrackItem item);

        /// <summary>
        /// Removes a collection of track items from the track.
        /// </summary>
        /// <param name="items">The collection of track items to remove.</param>
        void RemoveTrackItems(IEnumerable<ITrackItem> items);

        /// <summary>
        /// Gets the duration of the track, calculated by the longest track item.
        /// </summary>
        /// <returns>The duration of the track.</returns>
        TimeCode GetDuration();

        /// <summary>
        /// Cuts the item at the specified timeline frame.
        /// </summary>
        /// <param name="item">The item to cut.</param>
        /// <param name="timelineFrame">The timeline frame at which to cut the item.</param>
        void CutItem(ITrackItem item, double timelineFrame);

        /// <summary>
        /// Gets the track item at the specified timeline frame.
        /// </summary>
        /// <param name="timelineFrame">The timeline frame to get the item at.</param>
        /// <returns>The track item at the specified timeline frame.</returns>
        ITrackItem GetItemAtTimelineFrame(double timelineFrame);

        /// <summary>
        /// Generates a new Id for the track.
        /// </summary>
        /// <returns>The new Id.</returns>
        string GenerateNewId();
    }
}