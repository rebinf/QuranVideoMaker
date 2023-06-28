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

    }
}
