namespace QuranVideoMaker.Undo.UndoUnits
{
    public class MultipleUndoUnits : IUndoUnit
    {
        public string Name { get; }

        public List<IUndoUnit> UndoUnits { get; } = new List<IUndoUnit>();

        public MultipleUndoUnits()
        {
        }

        public MultipleUndoUnits(string name)
        {
            Name = name;
        }

        public MultipleUndoUnits(string name, List<IUndoUnit> undoUnits)
        {
            Name = name;
            UndoUnits = undoUnits;
        }

        public void Undo()
        {
            foreach (var undoUnit in UndoUnits)
            {
                undoUnit.Undo();
            }
        }

        public void Redo()
        {
            foreach (var undoUnit in UndoUnits)
            {
                undoUnit.Redo();
            }
        }
    }
}
