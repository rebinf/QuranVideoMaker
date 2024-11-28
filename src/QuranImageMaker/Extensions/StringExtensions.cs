namespace QuranImageMaker.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a string by the specified separator and keeps the separator as part of the resulting substrings.
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="separator">The separator to use for splitting the string.</param>
        /// <returns>An enumerable collection of substrings, each ending with the separator except the last one.</returns>
        public static IEnumerable<string> SplitAndKeep(this string s, string separator)
        {
            string[] obj = s.Split(new string[] { separator }, StringSplitOptions.None);

            for (int i = 0; i < obj.Length; i++)
            {
                string result = i == obj.Length - 1 ? obj[i] : obj[i] + separator;
                yield return result;
            }
        }
    }
}
