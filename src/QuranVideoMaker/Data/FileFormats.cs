namespace QuranVideoMaker.Data
{
    public static class FileFormats
    {
        public static IEnumerable<string> SupportedExportFormats = new List<string> { "mp4", "mov", "webm" };

        public static IEnumerable<string> SupportedVideoFormats = new List<string> { ".mp4", ".mov", ".webm" };

        public static IEnumerable<string> SupportedImageFormats = new List<string> { ".png", ".jpg", ".jpeg" };

        public static IEnumerable<string> SupportedAudioFormats = new List<string> { ".mp3", ".wav" };

        public static IEnumerable<string> AllSupportedFormats = new List<string> { ".mp4", ".mov", ".webm", ".png", ".jpg", ".jpeg", ".mp3", ".wav" };

        public static string AllSupportedFormatsOpenFileExtensions => string.Join(";", AllSupportedFormats.Select(x => "*" + x));

        public static string AllSupportedOpenAudioFileExtensions => string.Join(";", SupportedAudioFormats.Select(x => "*" + x));

        public static IEnumerable<string> SupportedFileFormats => SupportedVideoFormats.Concat(SupportedImageFormats).Concat(SupportedAudioFormats);
    }
}
