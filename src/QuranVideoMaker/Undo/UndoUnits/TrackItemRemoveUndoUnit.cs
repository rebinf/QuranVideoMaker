using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents an undo unit that can be used to undo the removal of a track item.
    /// </summary>
    public class TrackItemRemoveUndoUnit : IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        public string Name { get; } = "Remove Track Item";

        /// <summary>
        /// Gets the track items that were removed.
        /// </summary>
        public List<TrackAndItemData> Items { get; } = new List<TrackAndItemData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemRemoveUndoUnit"/> class.
        /// </summary>
        public TrackItemRemoveUndoUnit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemRemoveUndoUnit"/> class.
        /// </summary>
        /// <param name="track">The timeline track.</param>
        /// <param name="item">The track item.</param>
        public TrackItemRemoveUndoUnit(ITimelineTrack track, ITrackItem item)
        {
            Items.Add(new TrackAndItemData(track, item));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemRemoveUndoUnit"/> class.
        /// </summary>
        /// <param name="items">The track items.</param>
        public TrackItemRemoveUndoUnit(IEnumerable<TrackAndItemData> items)
        {
            Items.AddRange(items);
        }

        /// <inheritdoc/>
        public void Undo()
        {
            foreach (var item in Items)
            {
                item.Track.Items.Add(item.Item);
            }
        }

        /// <inheritdoc/>
        public void Redo()
        {
            foreach (var item in Items)
            {
                item.Track.Items.Remove(item.Item);
            }
        }
    }
}
