namespace AzeLib.Extensions
{
    public static class StringExt
    {
        public static string Truncate(this string source, int length)
        {
            if (source.Length > length)
                source = source.Substring(0, length);
            return source;
        }
    }
}
