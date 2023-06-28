using QuranVideoMaker.Data;

namespace QuranVideoMaker
{
    public static class TrackItemExtensions
    {
        public static double GetXPosition(this ITrackItem trackItem, int zoom)
        {
            var position = trackItem.Position.TotalFrames * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[zoom];
            return position;
        }

        public static double GetPlayXPosition(this ITrackItem trackItem, int zoom, double needleFrame)
        {
            var position = trackItem.GetXPosition(zoom);
            var playPosition = needleFrame + trackItem.Start.Frame;

            return playPosition;
        }

        public static double GetFadeInXPosition(this ITrackItem trackItem, int zoom)
        {
            var position = trackItem.FadeInFrame * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[zoom];
            return position;
        }

        public static double GetFadeOutXPosition(this ITrackItem trackItem, int zoom)
        {
            var position = trackItem.FadeOutFrame * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[zoom];
            return position;
        }

        public static double GetWidth(this ITrackItem trackItem, int zoom)
        {
            var length = trackItem.Duration.TotalFrames / Constants.TimelineZooms[zoom] * Constants.TimelinePixelsInSeparator;
            return length;
        }
    }
}
