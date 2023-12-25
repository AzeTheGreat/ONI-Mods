using System;
using System.Collections.Generic;
using System.Linq;

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

        // https://stackoverflow.com/questions/48387544/split-a-listint-into-groups-of-consecutive-numbers
        public static IEnumerable<IEnumerable<int>> GroupConsecutive(this IEnumerable<int> source)
        {
            using var e = source.GetEnumerator();
            for (bool more = e.MoveNext(); more;)
            {
                int first = e.Current, last = first, next;
                while ((more = e.MoveNext()) && (next = e.Current) > last && next - last == 1)
                    last = next;
                yield return Enumerable.Range(first, last - first + 1);
            }
        }

        public static IEnumerable<Tout> LinqByValue<Tsrc, Tcomp, Tout>(this IEnumerable<Tsrc> source, 
            Func<IEnumerable<Tsrc>, Tcomp, IEnumerable<Tout>> linqFunction, 
            Func<IEnumerable<Tsrc>, Tcomp> getValue) => linqFunction(source, getValue(source));

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => source ?? Array.Empty<T>();

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>.</summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains all source <see cref="KeyValuePair{TKey, TValue}"/>.</returns>
        public static Dictionary<TKey, TVal> ToDictionary<TKey, TVal>(this IEnumerable<KeyValuePair<TKey, TVal>> source) => source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
