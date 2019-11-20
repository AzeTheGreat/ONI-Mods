using Harmony;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(WorldInspector), nameof(WorldInspector.MassStringsReadOnly))]
    class MassStringFix_Patch
    {
        // Functions even on cached, fix later
        static void Postfix(string[] __result)
        {
            __result[0] = __result[0] + __result[1] + __result[2] + __result[3];
            __result[1] = __result[2] = __result[3] = string.Empty;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new System.Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool)})]
    class DrawEmptyStringFix_Patch
    {
        static bool Prefix(string text)
        {
            if (text == string.Empty)
                return false;

            return true;
        }
    }
}
