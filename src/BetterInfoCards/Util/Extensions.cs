namespace BetterInfoCards
{
    public static class Extensions
    {
        public static string RemoveCountSuffix(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsDigit(s[s.Length - 1]))
                return s;

            var i = s.LastIndexOf(" x ");
            return s.Substring(0, i != -1 ? i : s.Length);
        }
    }
}
