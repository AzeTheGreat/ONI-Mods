using AzeLib;
using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.ModCompatability
{
    class SolidElement : LogicSettingDispComp
    {
        [MyCmpGet]
        private TreeFilterable treeFilterable;

        public override string GetSetting()
        {
            Tag tag = treeFilterable.AcceptedTags.FirstOrDefault();

            string additionalTags = string.Empty;
            if (treeFilterable.AcceptedTags.Count > 1)
                additionalTags = " +" + (treeFilterable.AcceptedTags.Count - 1);

            return tag.GetAbbreviation() + additionalTags;
        }

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var solidElement = AccessTools.Method("SolidSensors.SolidConduitElementSensorConfig:DoPostConfigureComplete");
                if (solidElement != null) yield return solidElement;
            }

            static void Postfix(GameObject go) => go.AddComponent<SolidElement>();
        }
    }
}
