using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    public class DefaultNewProject : Project
    {
        public DefaultNewProject()
        {
            Tracks.Add(new TimelineTrack(TrackType.Quran, "Quran"));
            Tracks.Add(new TimelineTrack(TrackType.Video, "Video 1"));
            Tracks.Add(new TimelineTrack(TrackType.Video, "Video 2"));
            Tracks.Add(new TimelineTrack(TrackType.Audio, "Audio"));
        }
    }
}
