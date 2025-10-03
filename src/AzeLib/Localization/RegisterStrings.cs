using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Klei;

namespace AzeLib
{
    /// <summary>Inherit from this class to mark all <see langword="static"/> <see cref="LocString"/> fields for registration.</summary>
    /// <remarks>Supports nested classes.<br/>
    /// <see langword="static"/> members will be registered, but not included in the POT.<br/>
    /// <see langword="public static"/> members will be registered and included in the POT.</remarks>
    /// <typeparam name="T"><inheritdoc cref="SingletonBase{T}"/></typeparam>
    public abstract class AStrings<T> : AStringsBase<T> where T : AStrings<T> { }

    /// <summary>Inherit from this class to register translations without needing to define <see cref="LocString"/> fields.</summary>
    /// <remarks>Defined <see cref="LocString"/> fields set English translations.  Any <see cref="LocString"/> fields must be non-<see langword="public"/>.<br/>
    /// Additional translations can be defined exclusively in PO files and will still be loaded.<br/>
    /// Implement <see cref="GetPOTEntries"/> to generate keys in the PO template.</remarks>
    /// <typeparam name="T"><inheritdoc cref="SingletonBase{T}"/></typeparam>
    public abstract class AFieldlessStrings<T> : AStringsBase<T>, IAFieldlessStrings where T : AFieldlessStrings<T>
    {
        /// <inheritdoc cref="IAFieldlessStrings.GetPOTEntries"/>
        public virtual List<POTEntry> GetPOTEntries() => [];
    }

    interface IAFieldlessStrings
    {
        /// <summary>Override this method so <see cref="POTemplateGenerator"/> can write PO template entries for this class.</summary>
        /// <remarks>If this is not overriden, fieldless strings will still be loaded from PO files, but the PO template will not include potential keys for translators to reference.</remarks>
        /// <returns>A <see cref="List{T}"/> of <see cref="POTEntry"/> to be written.  Defaults to empty.</returns>
        List<POTEntry> GetPOTEntries();
    }

    /// <summary>A base class used for string registration. Do not inherit from this; use <see cref="AStrings{T}"/> or <see cref="AFieldlessStrings{T}"/>.</summary>
    public abstract class AStringsBase<T> : SingletonBase<T> where T : AStringsBase<T>
    {
        /// <summary>Tries to get the current translated <see cref="StringEntry"/> for a partial key registered through this type.</summary>
        /// <param name="partialKey">The partial key that will be used to search.  This is combined with <see cref="Type"/> and <see langword="namespace"/> information to get the full key.</param>
        /// <param name="translation">A <see cref="StringEntry"/> representing the translation.  <see langword="null"/> if not found.</param>
        /// <returns><see langword="true"/> if translation was loaded; <see langword="false"/> if no match for the key could be found.</returns>
        public static bool TryGetString(string partialKey, out StringEntry translation) => Strings.TryGet(AzeStrings.GetFullKey(typeof(T), partialKey), out translation);
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
                Localization.AddAssembly(rootType.Assembly);

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