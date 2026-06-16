using HarmonyLib;
using System;
using UnityEngine;

namespace BetterInfoCards.Util;

[HarmonyPatch(typeof(DetailsScreen), nameof(DetailsScreen.Refresh))]
static class SideScreenCrashLogger
{
    static void Finalizer(Exception __exception, GameObject go)
    {
        if (__exception is not null)
        {
            Debug.Log("Sidescreen Crash:");
            Debug.Log("Refresh GO: " + go + " (is null? )" + (go == null));
            Debug.Log("Exception: " + __exception);
        }
    }
}
