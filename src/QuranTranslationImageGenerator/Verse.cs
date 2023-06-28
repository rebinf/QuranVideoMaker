namespace QuranTranslationImageGenerator
{
    public struct Verse
    {
        public Guid Id { get; }

        public int ChapterNumber { get; set; }

        public int VerseNumber { get; set; }

        public string VerseText { get; set; }

        public Verse(Guid id, int chapterNumber, int verseNumber, string verseText)
        {
            Id = id;
            ChapterNumber = chapterNumber;
            VerseNumber = verseNumber;
            VerseText = verseText;
        }

        public VerseInfo ToVerseInfo()
        {
            return new VerseInfo(Id, ChapterNumber, VerseNumber, VerseText);
        }
    }
}
