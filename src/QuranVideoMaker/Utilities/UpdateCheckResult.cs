namespace QuranVideoMaker.Utilities
{
    public class UpdateCheckResult
    {
        public Version LatestVersion { get; set; } = new Version(0, 0, 1);

        public LatestReleaseInfo ReleaseInfo { get; set; }
    }
}
