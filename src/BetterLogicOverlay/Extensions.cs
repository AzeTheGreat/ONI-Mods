using AzeLib.Extensions;
using UnityEngine;

namespace BetterLogicOverlay
{
    static class Extensions
    {
        public static string GetAbbreviation(this Element element)
        {
            if (element == null)
                return null;

            var abbreviation = ElementAbbreviation.abbreviations[element.id];

            if (abbreviation == string.Empty || !Options.Opts.isTranslated)
                abbreviation = element.name;
            return abbreviation;
        }

        public static string GetAbbreviation(this Tag tag)
        {
            if (tag == null)
                return STRINGS.ELEMENTS.VOID.NAME;

            var element = ElementLoader.FindElementByHash((SimHashes)tag.GetHash());
            return element.GetAbbreviation() ?? STRINGS.UI.StripLinkFormatting(TagManager.GetProperName(tag)).Truncate(5);
        }

        public static GameObject GetGO(this ILogicUIElement logicUIElement)
        {
            var cell = logicUIElement.GetLogicUICell();
            return Grid.Objects[cell, (int)ObjectLayer.LogicGate] ?? Grid.Objects[cell, (int)ObjectLayer.Building];
        }
    }
}
