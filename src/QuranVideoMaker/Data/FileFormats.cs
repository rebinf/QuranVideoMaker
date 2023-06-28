namespace QuranVideoMaker.Data
{
    public static class FileFormats
    {
        public static IEnumerable<string> SupportedVideoFormats = new List<string> { ".mp4" };

        public static IEnumerable<string> SupportedImageFormats = new List<string> { ".png", ".jpg", ".jpeg" };

        public static IEnumerable<string> SupportedAudioFormats = new List<string> { ".mp3" };

        public static IEnumerable<string> AllSupportedFormats = new List<string> { ".mp4", ".png", ".jpg", ".jpeg", ".mp3" };

        public static string AllSupportedFormatsOpenFileExtensions => string.Join(";", AllSupportedFormats.Select(x => "*" + x));

        public static IEnumerable<string> SupportedFileFormats => SupportedVideoFormats.Concat(SupportedImageFormats).Concat(SupportedAudioFormats);
    }
}
