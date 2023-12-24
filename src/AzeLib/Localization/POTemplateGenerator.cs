using AzeLib.Extensions;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AzeLib
{
    /// <summary>Defines a PO template entry.</summary>
    /// <param name="partialKey"><inheritdoc cref="PartialKey" path="/summary"/></param>
    /// <param name="comment"><inheritdoc cref="Comment" path="/summary"/></param>
    public class POTEntry(string partialKey, string comment = null)
    {
        /// <summary>A partial key.  This should not include <see cref="Type"/> and <see cref="namespace"/> information.  Corresponds to "msgctxt" line in POTs.</summary>
        public string PartialKey { get; } = partialKey;
        /// <summary>An optional comment.  Corresponds to "#. " line in POTs.</summary>
        public string Comment { get; } = comment;
    }

#if DEBUG
    internal class POTemplateGenerator
    {
        internal static void GeneratePOT(Type locStringRoot, List<IAFieldlessStrings> fieldlessStrings, string outputDir)
        {
            outputDir = FileSystem.Normalize(outputDir);
            if (!FileUtil.CreateDirectory(outputDir, 5))
                return;

            var outputPath = FileSystem.Normalize(Path.Combine(outputDir, $"{locStringRoot.Namespace.ToLower()}_template.pot"));
            GeneratePOT(locStringRoot.Namespace, Assembly.GetAssembly(locStringRoot), outputPath, GetExtraStringsTree());

            Dictionary<string, object> GetExtraStringsTree() =>
                CreateStringsTree(
                    fieldlessStrings,
                    fs => fs.GetPOTEntries().ToDictionary(entry => entry.PartialKey, entry => entry as object),
                    fs => fs.GetType().Name);
        }

        private static void GeneratePOT(string lsNamespace, Assembly lsAssembly, string outputDir, Dictionary<string, object> extraStringsTree)
        {
            using (var sw = new StreamWriter(outputDir, false, new UTF8Encoding(false)))
            {
                WriteHeader(sw);
                WritePOT(lsNamespace.ToUpper(), sw, GetStringsTree());
            }

            Debug.Log("Generated AzePOT at " + outputDir);

            static void WriteHeader(StreamWriter sw)
            {
                sw.WriteLine("msgid \"\"");
                sw.WriteLine("msgstr \"\"");
                sw.WriteLine("\"Application: Oxygen Not Included\"");
                sw.WriteLine("\"POT Version: 2.0\"");
                sw.WriteLine();
            }

            Dictionary<string, object> GetStringsTree() =>
                CreateStringsTree(
                    Localization.CollectLocStringTreeRoots(lsNamespace, lsAssembly).ToList(),
                    Localization.MakeRuntimeLocStringTree,
                    type => type.Name)
                .Concat(extraStringsTree.EmptyIfNull())
                .ToDictionary();
        }

        private static void WritePOT(string path, StreamWriter sw, Dictionary<string, object> stringsTree)
        {
            foreach (var (key, value) in stringsTree.OrderBy(k => k.Key))
            {
                var fullKey = path + "." + key;

                if (value is string defaultTranslation)
                    WriteEntry(sw, fullKey, defaultTranslation);
                else if (value is POTEntry potEntry)
                    WriteEntry(sw, fullKey,
                        Strings.TryGet(AzeStrings.GetFullKey(fullKey), out var s) ? s : string.Empty,
                        potEntry.Comment);
                else
                    WritePOT(fullKey, sw, value as Dictionary<string, object>);
            }

            static void WriteEntry(StreamWriter sw, string fullKey, string defaultTranslation, string comment = null)
            {
                if (!comment.IsNullOrWhiteSpace())
                    sw.WriteLine("#. " + comment);
                sw.WriteLine("msgctxt \"{0}\"", fullKey);
                sw.WriteLine("msgid \"" + FixupString(defaultTranslation) + "\"");
                sw.WriteLine("msgstr \"\"");
                sw.WriteLine();

                static string FixupString(string str) =>
                    (str.IsNullOrWhiteSpace() ? AzeLocalization.EmptyTranslationPlaceholder : str)
                    .Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("’", "'")
                    .Replace("“", "\\\"")
                    .Replace("”", "\\\"")
                    .Replace("…", "...");
            }
        }

        private static Dictionary<string, object> CreateStringsTree<T>(List<T> collection, Func<T, Dictionary<string, object>> getTree, Func<T, string> getKey)
        {
            return collection
                .ToDictionary(x => getKey(x), x => getTree(x) as object)
                .Where(kvp => !isNullOrEmpty(kvp.Value))
                .ToDictionary();

            static bool isNullOrEmpty(object val) => val == null || (val is Dictionary<string, object> dict && !dict.Any());
        }
    }
#endif
}