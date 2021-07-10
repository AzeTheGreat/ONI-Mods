using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        // String declaration classes inherit from this to avoid explicitly declaring an empty category.
        // This is necessary so that "base" options are not given a "MISSING.STRINGS..." category.
        // TODO: Remove when PLib updates to add implicit categories.
        abstract public class BaseOpt
        {
            public static LocString CATEGORY = string.Empty;
        } 
    }
}