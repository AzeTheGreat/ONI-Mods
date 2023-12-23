using System;

namespace AzeLib
{
    public static class AzeStrings
    {
        private const string pathPrefix = "STRINGS.";

        public static string GetParentPath(Type type) => pathPrefix + type.Namespace.ToUpper() + ".";

        public static bool TryGet<T>(string partialKey, out StringEntry result) => Strings.TryGet(GetFullKey(typeof(T), partialKey), out result);

        public static string GetFullKey(Type type, string partialKey) => pathPrefix + type.FullName.Replace("+", ".").ToUpper() + "." + partialKey;
        public static string GetFullKey(string potPath) => pathPrefix + potPath;
    }
}
