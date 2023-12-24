using System;

namespace AzeLib
{
    internal static class AzeStrings
    {
        private const string pathPrefix = "STRINGS.";

        internal static string GetParentPath(Type type) => pathPrefix + type.Namespace.ToUpper() + ".";

        internal static string GetFullKey(Type type, string partialKey) => pathPrefix + type.FullName.Replace("+", ".").ToUpper() + "." + partialKey;
        internal static string GetFullKey(string potPath) => pathPrefix + potPath;
    }
}
