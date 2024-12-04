using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    public class TrackItemResizeUndoUnit : IUndoUnit
    {
        public string Name { get; } = "Resize Track Item";

        public List<TrackItemResizeData> Items { get; } = new List<TrackItemResizeData>();

        public TrackItemResizeUndoUnit()
        {
        }

        public TrackItemResizeUndoUnit(ITrackItem item)
        {
            Items.Add(new TrackItemResizeData(item));
        }

        public TrackItemResizeUndoUnit(TrackItemResizeData itemData)
        {
            Items.Add(itemData);
        }

        public void Undo()
        {
            foreach (var item in Items)
            {
                item.Undo();
            }
        }

        public void Redo()
        {
            foreach (var item in Items)
            {
                item.Redo();
            }
        }
    }
}
