using QuranVideoMaker.Data;

namespace QuranVideoMaker.ClipboardData
{
    public class TrackItemClipboardData
    {
        public string SourceTrackId { get; set; }

        public ITrackItem TrackItem { get; set; }

        public TrackItemClipboardData()
        {
        }

        public TrackItemClipboardData(string sourceTrackId, ITrackItem trackItem)
        {
            SourceTrackId = sourceTrackId;
            TrackItem = trackItem;
        }

        public static TrackItemClipboardData[] TrackItems(string sourceTrackId, IEnumerable<ITrackItem> trackItems)
        {
            var clipboardData = new TrackItemClipboardData[trackItems.Count()];

            for (int i = 0; i < trackItems.Count(); i++)
            {
                clipboardData[i] = new TrackItemClipboardData(sourceTrackId, trackItems.ElementAt(i));
            }

            return clipboardData;
        }
    }
}
