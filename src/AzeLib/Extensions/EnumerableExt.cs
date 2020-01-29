using System;
using System.Collections.Generic;

namespace AzeLib.Extensions
{
    public static class EnumerableExt
    {
        public static T FindNext<T>(this IEnumerable<T> e, T start, Func<T, bool> isTarget) where T: class
        {
            var list = new List<T>(e);
            int startIndex = list.FindIndex(x => x == start);

            for (int i = ++startIndex; i < list.Count; i++)
            {
                var item = list[i];
                if (isTarget(item))
                    return item;
            }

            return null;
        }

        public static T FindPrior<T>(this IEnumerable<T> e, T start, Func<T, bool> isTarget) where T : class
        {
            var list = new List<T>(e);
            int startIndex = list.FindIndex(x => x == start);

            for (int i = --startIndex; i >= 0; i--)
            {
                var item = list[i];
                if (isTarget(item))
                    return item;
            }

            return null;
        }
    }
}
