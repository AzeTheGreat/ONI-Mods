using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace NoPointlessScrollbars
{
    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.ConfigurePanelSize))]
    class DisableScrollbar_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var setActiveMethod = AccessTools.Method(typeof(GameObject), nameof(GameObject.SetActive));
            var subtractTarget = codes
                .First(i => i.Calls(setActiveMethod))
                .FindPrior(codes, i => i.OpCodeIs(OpCodes.Sub));

            // Source is rows >= max - 1, but for ideal functionality should be rows > max.
            // This is the simplest way to achieve the same results (rows >= max + 1).
            return codes
                .Manipulator(
                    i => i == subtractTarget,
                    _ => new CodeInstruction(OpCodes.Add));
        }

        // The size delta is set at the end of the method, so this has to occur after that does.
        static void Postfix(PlanScreen __instance)
        {
            var scrollRect = __instance.BuildingGroupContentsRect.GetComponent<ScrollRect>();
            var shouldShowScroll = scrollRect.verticalScrollbar.IsActive();

            scrollRect.vertical = shouldShowScroll;

            // If we remove the scrollbar, have to manually adjust the size to account for it.
            // I am not sure why this rect dimension is negative, but I'll take it.
            if (!shouldShowScroll)
                __instance.buildingGroupsRoot.sizeDelta += new Vector2(scrollRect.verticalScrollbar.rectTransform().rect.x, 0f);
        }
    }
}