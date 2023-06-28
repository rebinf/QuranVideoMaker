namespace QuranTranslationImageGenerator
{
    public class VerseCollection : List<VerseInfo>
    {
        public VerseRenderSettings RenderSettings { get; set; } = new VerseRenderSettings();
    }
}
