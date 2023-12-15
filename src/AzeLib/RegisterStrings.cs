using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AzeLib
{
    [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
    public abstract class RegisterStrings
    {
        protected const string d = ".";
        protected const string parentPath = "STRINGS.AZE.";

        private static List<Type> locStringRoots;

        // Only patch if the assembly actually has anything to translate.
        static bool Prepare()
        {
            locStringRoots = ReflectionHelpers.GetChildTypesOfType<RegisterStrings>().ToList();
            return locStringRoots.Count >= 1;
        }

        static void Postfix()
        {
            if (locStringRoots.FirstOrDefault() is Type rootType)
            {
                // Manually add the assembly, instead of calling Localization.RegisterForTranslation, to avoid the pointless CreateLocStringKeys call.
                Localization.AddAssembly(rootType.Namespace, rootType.Assembly);

                LoadStrings();
                UpdateStrings();
                GeneratePOTemplate(rootType);
            }
        }

        // Must manually load from "translations" because files in "strings" are always overloaded, even if they don't match the current locale.
        private static void LoadStrings()
        {
            // This is needed, instead of just Localization.GetLocale()?.Code, to work around the lack of locale declaration in the Russian translation.
            var code = Localization.GetLocaleForCode(Localization.GetCurrentLanguageCode())?.Code;

            var path = Path.Combine(AzeMod.UserMod.path, "translations", code + ".po");
            if (File.Exists(path))
                Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
        }

        // When strings are translated, the value of the LocString field is set, but the strings are never re-registered.
        // Thus any uses of Strings.Get return the untranslated value.
        // This calls LocString.CreateLocStringKeys to re-register the strings.
        static void UpdateStrings()
        {
            foreach (var type in locStringRoots)
            {
                var parentPath = "STRINGS." + type.Namespace.ToUpper() + ".";
                LocString.CreateLocStringKeys(type, parentPath);
            }
        }

        // PO templates are generated only for Debug builds to save load time and reduce clutter for end users.
        [System.Diagnostics.Conditional("DEBUG")]
        private static void GeneratePOTemplate(Type rootType)
        {
            // There's really no way to access MSBuild properties in C#.
            // This is the best we get: up three directories to get to ONI-Mods, then to the Translations folder.
            var transDir = Path.GetFullPath(Path.Combine(GetCallerPath(), "..", "..", "..", "Translations", rootType.Namespace));
            Localization.GenerateStringsTemplate(rootType, transDir);

            // This uses Roslyn to inject the path of this .cs file at compile time.
            static string GetCallerPath([CallerFilePath] string path = null) => path;
        }
    }
}