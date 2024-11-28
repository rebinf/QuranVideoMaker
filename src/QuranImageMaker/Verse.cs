namespace QuranImageMaker
{
    /// <summary>
    /// Represents a verse in the Quran.
    /// </summary>
    public struct Verse
    {
        /// <summary>
        /// Gets the unique identifier for the verse.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets or sets the chapter number of the verse.
        /// </summary>
        public int ChapterNumber { get; set; }

        /// <summary>
        /// Gets or sets the verse number within the chapter.
        /// </summary>
        public int VerseNumber { get; set; }

        /// <summary>
        /// Gets or sets the text of the verse.
        /// </summary>
        public string VerseText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Verse"/> struct.
        /// </summary>
        /// <param name="id">The unique identifier for the verse.</param>
        /// <param name="chapterNumber">The chapter number of the verse.</param>
        /// <param name="verseNumber">The verse number within the chapter.</param>
        /// <param name="verseText">The text of the verse.</param>
        public Verse(Guid id, int chapterNumber, int verseNumber, string verseText)
        {
            Id = id;
            ChapterNumber = chapterNumber;
            VerseNumber = verseNumber;
            VerseText = verseText;
        }

        /// <summary>
        /// Converts the current <see cref="Verse"/> to a <see cref="VerseInfo"/> object.
        /// </summary>
        /// <returns>A <see cref="VerseInfo"/> object containing the verse information.</returns>
        public VerseInfo ToVerseInfo()
        {
            return new VerseInfo(Id, ChapterNumber, VerseNumber, VerseText);
        }
    }
}
