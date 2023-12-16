using AzeLib;
using HarmonyLib;
using System.Collections.Generic;

namespace BetterLogicOverlay
{
    [HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CopyEntryToElement))]
    public static class ElementAbbreviation
    {
        public static Dictionary<SimHashes, string> abbreviations = [];

        static void Postfix(ElementLoader.ElementEntry entry, Element elem)
        {
            var id = GetRootElementID(entry.elementId);
            AzeStrings.TryGet<MYSTRINGS.ABBREVIATIONS>(id, out var abbreviation);
            abbreviations.Add(elem.id, abbreviation ?? string.Empty);
        }

        private static string GetRootElementID(string id)
        {
            var toStrip = new List<string>() { "Molten", "Liquid", "Solid", "Gas" };

            foreach (var str in toStrip)
                id = id.Replace(str, string.Empty);
            return id;
        }
    }
}
