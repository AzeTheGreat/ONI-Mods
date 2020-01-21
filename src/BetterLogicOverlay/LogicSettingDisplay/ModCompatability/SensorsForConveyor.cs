using AzeLib;
using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.ModCompatability
{
    class SolidTemp : ThresholdSwitchSetting
    {
        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var solidTemp = AccessTools.Method("SolidConduitTemperatureSensorConfig:DoPostConfigureComplete");
                if (solidTemp != null) yield return solidTemp;
            }

            static void Postfix(GameObject go) => go.AddComponent<SolidTemp>();
        }
    }

    class SolidDisease : GermSensorSetting
    {
        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var solidDisease = AccessTools.Method("SolidConduitDiseaseSensorConfig:DoPostConfigureComplete");
                if (solidDisease != null) yield return solidDisease;
            }

            static void Postfix(GameObject go) => go.AddComponent<SolidDisease>();
        }
    }

    class SolidElement : LogicSettingDispComp
    {
        [MyCmpGet]
        private TreeFilterable treeFilterable;

        public override string GetSetting()
        {
            Tag tag = treeFilterable.AcceptedTags.FirstOrDefault();

            if (tag == null)
                return STRINGS.ELEMENTS.VOID.NAME;

            string additionalTags = string.Empty;
            if (treeFilterable.AcceptedTags.Count > 1)
                additionalTags = " +" + (treeFilterable.AcceptedTags.Count - 1);

            Element element = ElementLoader.FindElementByHash((SimHashes)tag.GetHash());

            if (element == null)
                return STRINGS.UI.StripLinkFormatting(TagManager.GetProperName(tag)).Truncate(5) + additionalTags;

            return element.GetAbbreviation() + additionalTags;
        }

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var solidElement = AccessTools.Method("SolidConduitElementSensorConfig:DoPostConfigureComplete");
                if (solidElement != null) yield return solidElement;
            }

            static void Postfix(GameObject go) => go.AddComponent<SolidElement>();
        }
    }
}
