#if DEBUG

using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AzeLib
{
    internal class POTemplateGenerator
    {
        internal static void GeneratePOT(Type locStringRoot, List<IAFieldlessStrings> fieldlessStrings, string outputDir)
        {
            outputDir = FileSystem.Normalize(outputDir);
            if (!FileUtil.CreateDirectory(outputDir, 5))
                return;

            var outputPath = FileSystem.Normalize(Path.Combine(outputDir, $"{locStringRoot.Namespace.ToLower()}_template.pot"));
            GeneratePOT(locStringRoot.Namespace, Assembly.GetAssembly(locStringRoot), outputPath, GetExtraStringsTree());

            Dictionary<string, object> GetExtraStringsTree()
            {
                var extraStringsTree = new Dictionary<string, object>();
                foreach (var instance in fieldlessStrings)
                {
                    var instanceStringsTree = new Dictionary<string, object>();
                    foreach (var ls in instance.GetLocStrings())
                        instanceStringsTree[ls.key.String] = ls.text;

                    if(instanceStringsTree.Any())
                        extraStringsTree[instance.GetType().Name] = instanceStringsTree;
                }
                return extraStringsTree;
            }
        }

        private static void GeneratePOT(string lsNamespace, Assembly lsAssembly, string outputDir, Dictionary<string, object> extraStringsTree)
        {
            using (var sw = new StreamWriter(outputDir, false, new UTF8Encoding(false)))
            {
                WriteHeader(sw);
                WritePOT(lsNamespace, sw, GetStringsTree());
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

            Dictionary<string, object> GetStringsTree()
            {
                var stringsTree = new Dictionary<string, object>();
                foreach (var rootType in Localization.CollectLocStringTreeRoots(lsNamespace, lsAssembly))
                {
                    var rootStringsTree = Localization.MakeRuntimeLocStringTree(rootType);
                    if (rootStringsTree.Any())
                        stringsTree[rootType.Name] = rootStringsTree;
                }

                if (extraStringsTree != null)
                    stringsTree = stringsTree.Concat(extraStringsTree).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                return stringsTree;
            }
        }

        private static void WritePOT(string path, StreamWriter sw, Dictionary<string, object> stringsTree)
        {
            foreach (var (key, value) in stringsTree.OrderBy(k => k.Key))
            {
                var fullKey = path + "." + key;

                if (value is string defaultTranslation)
                    WriteEntry(sw, fullKey, FixupString(defaultTranslation));
                else
                    WritePOT(fullKey, sw, value as Dictionary<string, object>);
            }

            static void WriteEntry(StreamWriter sw, string fullKey, string defaultTranslation)
            {
                sw.WriteLine("msgctxt \"{0}\"", fullKey);
                sw.WriteLine("msgid \"" + defaultTranslation + "\"");
                sw.WriteLine("msgstr \"\"");
                sw.WriteLine();
            }

            static string FixupString(string str)
            {
                return str.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("’", "'")
                    .Replace("“", "\\\"")
                    .Replace("”", "\\\"")
                    .Replace("…", "...");
            }
        }
    }
}

#endif