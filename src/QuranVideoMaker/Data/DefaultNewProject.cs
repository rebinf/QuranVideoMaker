namespace QuranVideoMaker.Data
{
    public class DefaultNewProject : Project
    {
        public DefaultNewProject()
        {
            Tracks.Add(new TimelineTrack(TimelineTrackType.Quran, "Quran"));
            Tracks.Add(new TimelineTrack(TimelineTrackType.Video, "Video 1"));
            Tracks.Add(new TimelineTrack(TimelineTrackType.Video, "Video 2"));
            Tracks.Add(new TimelineTrack(TimelineTrackType.Audio, "Audio"));
        }
    }
}
