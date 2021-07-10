using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DefaultBuildingSettings
{
    [HarmonyPatch()]
    class SwitchesOff_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(LogicSwitchConfig), nameof(LogicSwitchConfig.DoPostConfigureComplete));
            yield return AccessTools.Method(typeof(SwitchConfig), nameof(SwitchConfig.DoPostConfigureComplete));
        }

        static void Postfix(GameObject go)
        {
            var switchComp = go.GetComponent<Switch>();
            switchComp.defaultState = !Options.Opts.SwitchesOff;
        }
    }
}
