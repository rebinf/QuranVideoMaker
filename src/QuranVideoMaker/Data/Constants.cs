using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    public static class Constants
    {
        /// <summary>
        /// The timeline zooms (frames per section)
        /// </summary>
        public static double[] TimelineZooms =
        {
            7680,
            7680,
            3840,
            1920,
            960,
            480,
            240,
            120,
            60,
            30,
            20,
            10,
            5,
            1,
        };

        public static int TimelineZoomMax { get { return TimelineZooms.Length - 1; } }

        public static double TimelinePixelsInSeparator = 60;

        public static IEnumerable<string> SupportedVideoFormats = new List<string> { ".mp4" };

        public static IEnumerable<string> SupportedImageFormats = new List<string> { ".png", ".jpg", ".jpeg" };

        public static IEnumerable<string> SupportedAudioFormats = new List<string> { ".mp3" };

        public static IEnumerable<string> AllSupportedFormats = new List<string> { ".mp4", ".png", ".jpg", ".jpeg", ".mp3" };

        public static string AllSupportedFormatsOpenFileExtensions => string.Join(";", AllSupportedFormats.Select(x => "*" + x));

        public static IEnumerable<string> SupportedFileFormats => SupportedVideoFormats.Concat(SupportedImageFormats).Concat(SupportedAudioFormats);
    }
}
