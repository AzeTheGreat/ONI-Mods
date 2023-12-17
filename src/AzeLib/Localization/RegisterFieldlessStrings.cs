using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzeLib
{
    [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
    public abstract class RegisterFieldlessStrings
    {
        private static List<Type> fieldlessStringRoots;

        static bool Prepare()
        {
            fieldlessStringRoots = ReflectionHelpers.GetChildTypesOfType<RegisterFieldlessStrings>().ToList();
            return fieldlessStringRoots.Count >= 1;
        }

        static void Postfix()
        {
            if(Localization.GetSelectedLanguageType() == Localization.SelectedLanguageType.None)
                SetStringsDBEntries();
            if (AzeLocalization.TryLoadTranslations(out var translations))
                SetFieldlessStringsDBEntries(translations);
        }

        static void SetStringsDBEntries()
        {
            foreach (var type in fieldlessStringRoots)
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