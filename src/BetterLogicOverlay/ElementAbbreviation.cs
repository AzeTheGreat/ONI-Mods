using AzeLib;
using HarmonyLib;
using System.Collections.Generic;

namespace BetterLogicOverlay
{
    [HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CopyEntryToElement))]
    public static class ElementAbbreviation
    {
        private static Dictionary<SimHashes, string> abbreviations = [];

        public static string GetAbbreviation(SimHashes hash) => abbreviations[hash];

        static void Postfix(ElementLoader.ElementEntry entry, Element elem)
        {
            var id = StripElementModifiers(entry.elementId);
            AzeStrings.TryGet<ABBREVIATIONS>(id, out var result);
            var abbreviation = result?.String;

            if (abbreviation.IsNullOrWhiteSpace() || !Options.Opts.isTranslated)
                abbreviation = StripElementModifiers(elem.name);

            abbreviations.Add(elem.id, abbreviation);
        }

        private static string StripElementModifiers(string id)
        {
            var toStrip = new List<string>() { "Molten", "Liquid", "Solid", "Gas" };

            foreach (var str in toStrip)
                id = id.Replace(str, string.Empty);
            return id;
        }
    }
}
