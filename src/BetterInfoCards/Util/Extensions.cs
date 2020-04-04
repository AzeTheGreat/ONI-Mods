namespace BetterInfoCards
{
    public static class Extensions
    {
        public static string RemoveCountSuffix(this string s)
        {
            var i = s.LastIndexOf(" x ");
            return s.Substring(0, i != -1 ? i : s.Length);
        }
    }
}
