using Harmony;
using UnityEngine;

namespace BetterInfoCards
{
    //// The game keeps mass strings in an array, but this leads to moving a lot more stuff around in my code and performance hits
    //// This condenses the string info into the first part of the array.
    //[HarmonyPatch(typeof(WorldInspector), nameof(WorldInspector.MassStringsReadOnly))]
    //class MassStringFix_Patch
    //{
    //    //Functions even on cached, fix later
    //    static void Postfix(string[] __result)
    //    {
    //        __result[0] = __result[0] + __result[1] + __result[2] + __result[3];
    //        __result[1] = __result[2] = __result[3] = string.Empty;
    //    }
    //}

    //// This prevents empty strings from ever being drawn.
    //[HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new System.Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
    //class DrawEmptyStringFix_Patch
    //{
    //    static bool Prefix(string text)
    //    {
    //        if (text == string.Empty)
    //            return false;

    //        return true;
    //    }
    //}

    //// Normally the game displays the third item in the array, or all if it's not vacuum
    //// This sets the 3 to 0 so that it writes it even if vacuum
    //[HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
    //class VacuumBreathableFix_Patch
    //{

    //}
}
