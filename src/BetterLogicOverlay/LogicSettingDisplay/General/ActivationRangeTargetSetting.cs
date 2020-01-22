using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class ActivationRangeTargetSetting : LogicSettingDispComp
    {
        [MyCmpGet] protected IActivationRangeTarget activationTarget;

        public override string GetSetting()
        {
            string unit = STRINGS.UI.UNITSUFFIXES.PERCENT;
            return activationTarget.DeactivateValue + unit + " - " + activationTarget.ActivateValue + unit;
        }

        public static void AddToDef(BuildingDef def)
        {
            var go = def.BuildingComplete;
            var component = go.AddComponent<ActivationRangeTargetSetting>();
        }
    }
}
