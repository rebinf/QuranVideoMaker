using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    public class TrackItemResizeData
    {
        public ITrackItem Item { get; set; }

        public TimeCode OldStart { get; set; }

        public TimeCode NewStart { get; set; }

        public TimeCode OldEnd { get; set; }

        public TimeCode NewEnd { get; set; }

        public TimeCode OldPosition { get; set; }

        public TimeCode NewPosition { get; set; }

        public TrackItemResizeData()
        {
        }

        public TrackItemResizeData(ITrackItem item)
        {
            Item = item;
        }

        public TrackItemResizeData(ITrackItem item, TimeCode oldPosition, TimeCode oldStart, TimeCode oldEnd)
        {
            Item = item;
            OldPosition = oldPosition;
            OldStart = oldStart;
            OldEnd = oldEnd;

            // Set the new values to the old values, so if the undo is called, it will revert to the original values.
            NewPosition = item.Position;
            NewStart = item.Start;
            NewEnd = item.End;
        }

        public void Undo()
        {
            Item.Start = OldStart;
            Item.End = OldEnd;
            Item.Position = OldPosition;
        }

        public void Redo()
        {
            Item.Start = NewStart;
            Item.End = NewEnd;
            Item.Position = NewPosition;
        }
    }
}
