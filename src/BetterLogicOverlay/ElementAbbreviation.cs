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
            LocString abbreviation = Traverse.Create(typeof(MYSTRINGS.ABBREVIATIONS)).Field(entry.elementId)?.GetValue<LocString>() ?? string.Empty;
            abbreviations.Add(elem.id, abbreviation);
        }
    }
}
