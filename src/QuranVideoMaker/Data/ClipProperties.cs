using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    public class ClipProperties
    {
        public TrackType TrackType { get; set; }

        public TrackItemType ItemType { get; set; }

        public TimeCode Length { get; set; }

        public double FPS { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool UnlimitedLength { get; set; }
    }
}
