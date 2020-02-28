using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace NoPointlessScrollbars
{
    [HarmonyPatch(typeof(PlanScreen), "ConfigurePanelSize")]
    class DisableScrollbar_Patch
    {
        private static int rows;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var ceilMethod = AccessTools.Method(typeof(Mathf), nameof(Mathf.CeilToInt));

            foreach (var i in instructions)
            {
                yield return i;
                if (i.Is(OpCodes.Call, ceilMethod))
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DisableScrollbar_Patch), nameof(DisableScrollbar_Patch.Patch)));    
            }
        }

        static void Postfix(PlanScreen __instance, int ___buildGrid_maxRowsBeforeScroll)
        {
            bool shouldShowScroll = rows >= ___buildGrid_maxRowsBeforeScroll + 1;

            __instance.BuildingGroupContentsRect.GetComponent<ScrollRect>().vertical = shouldShowScroll;

            if (!shouldShowScroll)
                // Couldn't find any relevant numbers in the source so I guess I too get to use magic numbers...
                __instance.buildingGroupsRoot.sizeDelta += new Vector2(-13f, 0f);
        }

        private static int Patch(int rowCount) => rows = rowCount;
    }

    [HarmonyPatch(typeof(PlanScreen), "OnSpawn")]
    class FixHeight_Patch
    {
        private static void Postfix(ref float ___buildGrid_bg_borderHeight)
        {
            // I have no idea why * 1.5 looks right, logically it should be * 2...
            // Unless the very top header is included, then it makes sense...
            ___buildGrid_bg_borderHeight *= 1.5f;
        }
    }
}