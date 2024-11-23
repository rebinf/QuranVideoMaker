namespace QuranVideoMaker.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T[]> Split<T>(this IEnumerable<T> list, int parts)
        {
            var result = list.Chunk(parts).ToList();

            return result;
        }
    }
}
