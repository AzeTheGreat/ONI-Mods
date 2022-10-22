using AzeLib.Extensions;
using BetterLogicOverlay.LogicSettingDisplay;
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

        public static LogicLabelSetting GetLogicLabelSetting(this ILogicUIElement logicUIElement)
        {
            var cell = logicUIElement.GetLogicUICell();

            var logic = Grid.Objects[cell, (int)ObjectLayer.LogicGate];
            if (logic && TryGetLabel(logic, out var logicLabel))
                return logicLabel;

            var building = Grid.Objects[cell, (int)ObjectLayer.Building];
            if (building && TryGetLabel(building, out var buildingLabel))
                return buildingLabel;

            return null;

            bool TryGetLabel(GameObject go, out LogicLabelSetting label) => go.TryGetComponent(out label) && label.ContainsLogicCell(cell);
        }
    }
}
