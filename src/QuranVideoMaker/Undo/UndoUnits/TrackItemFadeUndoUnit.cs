using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents an undo unit for track item fade actions.
    /// </summary>
    public class TrackItemFadeUndoUnit : IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        public string Name { get; } = "Track Item Fade";

        /// <summary>
        /// Gets or sets the track item associated with this undo unit.
        /// </summary>
        public ITrackItem Item { get; set; }

        /// <summary>
        /// Gets or sets the old fade-in frame value.
        /// </summary>
        public double OldFadeInFrame { get; set; }

        /// <summary>
        /// Gets or sets the old fade-out frame value.
        /// </summary>
        public double OldFadeOutFrame { get; set; }

        /// <summary>
        /// Gets or sets the new fade-in frame value.
        /// </summary>
        public double NewFadeInFrame { get; set; }

        /// <summary>
        /// Gets or sets the new fade-out frame value.
        /// </summary>
        public double NewFadeOutFrame { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemFadeUndoUnit"/> class.
        /// </summary>
        public TrackItemFadeUndoUnit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemFadeUndoUnit"/> class with specified parameters.
        /// </summary>
        /// <param name="item">The track item associated with this undo unit.</param>
        /// <param name="oldFadeInFrame">The old fade-in frame value.</param>
        /// <param name="oldFadeOutFrame">The old fade-out frame value.</param>
        public TrackItemFadeUndoUnit(ITrackItem item, double oldFadeInFrame, double oldFadeOutFrame)
        {
            Item = item;
            OldFadeInFrame = oldFadeInFrame;
            OldFadeOutFrame = oldFadeOutFrame;
        }

        /// <summary>
        /// Undoes the fade action by restoring the old fade-in and fade-out frame values.
        /// </summary>
        public void Undo()
        {
            Item.FadeInFrame = OldFadeInFrame;
            Item.FadeOutFrame = OldFadeOutFrame;
        }

        /// <summary>
        /// Redoes the fade action by applying the new fade-in and fade-out frame values.
        /// </summary>
        public void Redo()
        {
            Item.FadeInFrame = NewFadeInFrame;
            Item.FadeOutFrame = NewFadeOutFrame;
        }
    }
}
