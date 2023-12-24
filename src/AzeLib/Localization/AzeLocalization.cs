using AzeLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AzeLib
{
    internal static class AzeLocalization
    {
        // Empty strings in POTs causes editor issues; this works around the issue.
        internal const string EmptyTranslationPlaceholder = "-";

        const string TranslationFolder = "Translations";

        internal static bool TryLoadTranslations(out Dictionary<string, string> translations)
        {
            var path = Path.Combine(AzeMod.UserMod.path, TranslationFolder, Localization.GetLocale()?.Code + ".po");
            if (File.Exists(path))
            {
                translations = Localization.LoadStringsFile(path, false)
                    .Where(kvp => kvp.Value != EmptyTranslationPlaceholder)
                    .ToDictionary();
                return true;
            }

            translations = null;
            return false;
        }

        // PO templates are generated only for Debug builds to save load time and reduce clutter for end users.
        internal static void GeneratePOTemplate(Type rootType, List<IAFieldlessStrings> fieldlessStrings)
        {
        #if DEBUG
            if (Localization.GetSelectedLanguageType() != Localization.SelectedLanguageType.None)
                return;

            // There's really no way to access MSBuild properties in C#.
            // This is the best we get: up three directories to get to ONI-Mods, then to the Translations folder.
            var transDir = Path.GetFullPath(Path.Combine(GetCallerPath(), "..", "..", "..", "..", TranslationFolder, rootType.Namespace));
            POTemplateGenerator.GeneratePOT(rootType, fieldlessStrings, transDir);

            // This uses Roslyn to inject the path of this .cs file at compile time.
            static string GetCallerPath([CallerFilePath] string path = null) => path;
        #endif
        }
    }
}