using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuranVideoMaker.Undo
{
    /// <summary>
    /// Interface for an undo engine that manages undo and redo operations.
    /// </summary>
    public interface IUndoEngine : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs before an undo operation is performed.
        /// </summary>
        event EventHandler<IUndoUnit> OnUndoing;

        /// <summary>
        /// Occurs after an undo operation is performed.
        /// </summary>
        event EventHandler<IUndoUnit> OnUndone;

        /// <summary>
        /// Gets the stack of undo units.
        /// </summary>
        Stack<IUndoUnit> UndoStack { get; }

        /// <summary>
        /// Gets the stack of redo units.
        /// </summary>
        Stack<IUndoUnit> RedoStack { get; }

        /// <summary>
        /// Gets a value indicating whether an undo operation can be performed.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Gets a value indicating whether a redo operation can be performed.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Performs an undo operation.
        /// </summary>
        void Undo();

        /// <summary>
        /// Performs a redo operation.
        /// </summary>
        void Redo();

        /// <summary>
        /// Adds an undo unit to the undo stack.
        /// </summary>
        /// <param name="undoUnit">The undo unit to add.</param>
        void AddUndoUnit(IUndoUnit undoUnit);

        /// <summary>
        /// Clears the undo stack.
        /// </summary>
        void ClearUndoStack();

        /// <summary>
        /// Clears the redo stack.
        /// </summary>
        void ClearRedoStack();

        /// <summary>
        /// Clears both the undo and redo stacks.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        void OnPropertyChanged([CallerMemberName] string propertyName = null);
    }
}
