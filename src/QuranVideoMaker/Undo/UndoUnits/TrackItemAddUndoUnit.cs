using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents an undo unit that can be used to undo the addition of a track item.
    /// </summary>
    public class TrackItemAddUndoUnit : IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        public string Name { get; } = "Add Track Item";

        /// <summary>
        /// Gets the track items that were added.
        /// </summary>
        public List<TrackAndItemData> Items { get; } = new List<TrackAndItemData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemAddUndoUnit"/> class.
        /// </summary>
        public TrackItemAddUndoUnit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemAddUndoUnit"/> class.
        /// </summary>
        /// <param name="track">The timeline track.</param>
        /// <param name="trackItem">The track item.</param>
        public TrackItemAddUndoUnit(ITimelineTrack track, ITrackItem item)
        {
            Items.Add(new TrackAndItemData(track, item));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemAddUndoUnit"/> class.
        /// </summary>
        /// <param name="items">The track items.</param>
        public TrackItemAddUndoUnit(IEnumerable<TrackAndItemData> items)
        {
            Items.AddRange(items);
        }

        /// <inheritdoc/>
        public void Undo()
        {
            foreach (var item in Items)
            {
                item.Track.Items.Remove(item.Item);
            }
        }

        /// <inheritdoc/>
        public void Redo()
        {
            foreach (var item in Items)
            {
                item.Track.Items.Add(item.Item);
            }
        }
    }
}
