namespace QuranImageMaker
{
    /// <summary>
    /// Represents a group of drawing information with associated rendering settings.
    /// </summary>
    public class DrawGroup
    {
        /// <summary>
        /// Gets or sets the rendering settings for the verses.
        /// </summary>
        public VerseRenderSettings RenderSettings { get; set; }

        /// <summary>
        /// Gets or sets the list of drawing information.
        /// </summary>
        public List<DrawInfo> Draws { get; set; } = new List<DrawInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawGroup"/> class with the specified rendering settings.
        /// </summary>
        /// <param name="renderSettings">The rendering settings for the verses.</param>
        public DrawGroup(VerseRenderSettings renderSettings)
        {
            RenderSettings = renderSettings;
        }
    }
}
