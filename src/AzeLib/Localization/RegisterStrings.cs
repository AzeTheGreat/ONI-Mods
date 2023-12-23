using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzeLib
{
    public abstract class AStrings<T> where T : AStrings<T>
    {

    }

    public abstract class AFieldlessStrings<T> : SingletonBase<T>, IAFieldlessStrings where T : AFieldlessStrings<T>
    {
        public virtual List<POTEntry> GetPOTEntries() => [];
    }

    interface IAFieldlessStrings
    {
        List<POTEntry> GetPOTEntries();
    }

    [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
    static class RegisterStrings
    {
        private static List<Type> locStringRoots;
        private static List<Type> fieldlessStringRoots;
        private static List<IAFieldlessStrings> fieldlessStrings;

        // Only patch if the assembly actually has anything to translate.
        static bool Prepare()
        {
            locStringRoots = ReflectionHelpers.GetChildTypesOfGenericType(typeof(AStrings<>)).ToList();

            fieldlessStringRoots = ReflectionHelpers.GetChildTypesOfGenericType(typeof(AFieldlessStrings<>)).ToList();
            fieldlessStrings = fieldlessStringRoots.Select(SingletonHelper<IAFieldlessStrings>.GetInstance).ToList();

            return locStringRoots.Any() || fieldlessStringRoots.Any();
        }

        static void Postfix()
        {
            var translationsLoaded = AzeLocalization.TryLoadTranslations(out var translations);
            var rootType = locStringRoots.FirstOrDefault();

            if (rootType != null)
            {
                // Manually add the assembly, instead of calling Localization.RegisterForTranslation, to avoid the pointless CreateLocStringKeys call.
                Localization.AddAssembly(rootType.Namespace, rootType.Assembly);

                if (translationsLoaded)
                    SetLocStringFields(translations);
                SetStringsDBEntries(locStringRoots);
            }

            // If the default language is used, there will be no .po file to load.
            // Instead, register based off the English defaults in these types.
            if (Localization.GetSelectedLanguageType() == Localization.SelectedLanguageType.None)
                SetStringsDBEntries(fieldlessStringRoots);
            else if (translationsLoaded)
                SetFieldlessStringsDBEntries(translations);

            AzeLocalization.GeneratePOTemplate(rootType, fieldlessStrings);
        }

        // Must manually load from "translations" because files in "strings" are always overloaded, even if they don't match the current locale.
        private static void SetLocStringFields(Dictionary<string, string> translations) => Localization.OverloadStrings(translations);

        // When strings are translated, the value of the LocString field is set, but the strings are never re-registered.
        // Thus any uses of Strings.Get return the untranslated value.
        // This calls LocString.CreateLocStringKeys to re-register the strings.
        private static void SetStringsDBEntries(List<Type> types)
        {
            foreach (var type in types)
                LocString.CreateLocStringKeys(type, AzeStrings.GetParentPath(type));
        }

        private static void SetFieldlessStringsDBEntries(Dictionary<string, string> translations)
        {
            foreach (var type in fieldlessStringRoots)
            {
                var targetType = type.Name + ".";
                var translationsForType = translations.Where(kvp => kvp.Key.Contains(targetType));
                foreach (var kvp in translationsForType)
                {
                    var partialKey = kvp.Key.Substring(kvp.Key.IndexOf(targetType) + targetType.Length);
                    Strings.Add(AzeStrings.GetFullKey(type, partialKey), kvp.Value);
                }
            }
        }
    }
}