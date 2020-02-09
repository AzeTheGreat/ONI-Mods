using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace NoPointlessScrollbars
{
    [HarmonyPatch(typeof(PlanScreen), "ConfigurePanelSize")]
    class DisableScrollbar_Patch
    {
        private static void Postfix(PlanScreen __instance, int ___buildGrid_maxRowsBeforeScroll)
        {
            int buttons = __instance.GroupsTransform.childCount;
            for (int i = 0; i < __instance.GroupsTransform.childCount; i++)
            {
                if (!__instance.GroupsTransform.GetChild(i).gameObject.activeSelf)
                    buttons--;
            }

            int rows = Mathf.CeilToInt(buttons / 3f);
            bool shouldShowScroll = rows >= ___buildGrid_maxRowsBeforeScroll + 1;

            __instance.BuildingGroupContentsRect.GetComponent<ScrollRect>().vertical = shouldShowScroll;

            if (!shouldShowScroll)
                // Couldn't find any relevant numbers in the source so I guess I too get to use magic numbers...
                __instance.buildingGroupsRoot.sizeDelta += new Vector2(-13f, 0f);
        }
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