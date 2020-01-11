using System.Collections.Generic;

namespace BetterInfoCards
{
    public static class Extensions
    {
        public static string RemoveCountSuffix(this string text)
        {
            int i;
            bool wasDigit = false;
            for (i = text.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(text[i]))
                    wasDigit = true;
                else
                    break;
            }

            if (wasDigit)
                text = text.Remove(i - 2, text.Length - i + 2);

            return text;
        }
    }
}
