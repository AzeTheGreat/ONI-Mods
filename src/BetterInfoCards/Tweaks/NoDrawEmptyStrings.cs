using Harmony;
using System;
using UnityEngine;

namespace BetterInfoCards.Util
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
    class NoDrawEmptyStrings
    {
        // DrawText gets called with empty strings frequently, which is a waste of processing.
        // These originate from the MassStringsReadOnly part of UpdateHoverElements.
        static bool Prefix(string text) => !text.IsNullOrWhiteSpace();
    }
}
