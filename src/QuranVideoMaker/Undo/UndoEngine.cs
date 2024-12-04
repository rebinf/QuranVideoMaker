using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuranVideoMaker.Undo
{
    /// <summary>
    /// Class that represents an undo engine that manages undo and redo operations.
    /// </summary>
    public class UndoEngine : IUndoEngine
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public event EventHandler<IUndoUnit> OnUndoing;

        /// <inheritdoc />
        public event EventHandler<IUndoUnit> OnUndone;

        /// <inheritdoc />
        public Stack<IUndoUnit> UndoStack { get; } = new Stack<IUndoUnit>();

        /// <inheritdoc />
        public Stack<IUndoUnit> RedoStack { get; } = new Stack<IUndoUnit>();

        /// <inheritdoc />
        public bool CanUndo { get { return UndoStack.Count != 0; } }

        /// <inheritdoc />
        public bool CanRedo { get { return RedoStack.Count != 0; } }

        /// <summary>
        /// Gets the instance of the undo engine.
        /// </summary>
        public static UndoEngine Instance { get; } = new UndoEngine();

        /// <inheritdoc />
        public void Undo()
        {
            if (CanUndo)
            {
                var undoUnit = UndoStack.Pop();
                OnUndoing?.Invoke(this, undoUnit);
                undoUnit.Undo();
                RedoStack.Push(undoUnit);
                OnUndone?.Invoke(this, undoUnit);

                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));
            }
        }

        /// <inheritdoc />
        public void Redo()
        {
            if (CanRedo)
            {
                var redoUnit = RedoStack.Pop();
                OnUndoing?.Invoke(this, redoUnit);
                redoUnit.Redo();
                UndoStack.Push(redoUnit);
                OnUndone?.Invoke(this, redoUnit);

                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));
            }
        }

        /// <inheritdoc />
        public void AddUndoUnit(IUndoUnit undoUnit)
        {
            UndoStack.Push(undoUnit);
        }

        /// <inheritdoc />
        public void ClearRedoStack()
        {
            RedoStack.Clear();
        }

        /// <inheritdoc />
        public void ClearUndoStack()
        {
            UndoStack.Clear();
        }

        /// <inheritdoc />
        public void ClearAll()
        {
            ClearUndoStack();
            ClearRedoStack();
        }

        /// <inheritdoc />
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
