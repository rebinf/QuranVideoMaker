namespace QuranVideoMaker.Undo
{
    /// <summary>
    /// Represents an undo unit that can be used to undo an action.
    /// </summary>
    public interface IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Undoes the action represented by this undo unit.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redoes the action represented by this undo unit.
        /// </summary>
        void Redo();
    }
}