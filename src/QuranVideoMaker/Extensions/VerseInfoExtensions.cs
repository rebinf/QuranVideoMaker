using QuranImageMaker;

namespace QuranVideoMaker.Extensions
{
    public static class VerseInfoExtensions
    {
        public static void UpdateVerseParts(this IEnumerable<VerseInfo> verses)
        {
            foreach (var verseGroup in verses.ToArray().GroupBy(x => x.ChapterNumber + x.VerseNumber))
            {
                if (verseGroup.Count() > 1)
                {
                    int i = 1;

                    foreach (var verse in verseGroup)
                    {
                        verse.VersePart = i;
                        i++;
                    }
                }
            }
        }
    }
}
