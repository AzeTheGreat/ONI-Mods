using HarmonyLib;
using System.Collections.Generic;

namespace BetterLogicOverlay
{
    [HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CopyEntryToElement))]
    public static class ElementAbbreviation
    {
        public static Dictionary<SimHashes, LocString> abbreviations = new Dictionary<SimHashes, LocString>();

        static void Postfix(ElementLoader.ElementEntry entry, Element elem)
        {
            var id = GetRootElementID(entry.elementId);
            var abbreviation = Traverse.Create(typeof(MYSTRINGS.ABBREVIATIONS)).Field(id)?.GetValue<LocString>() ?? string.Empty;
            abbreviations.Add(elem.id, abbreviation);
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
