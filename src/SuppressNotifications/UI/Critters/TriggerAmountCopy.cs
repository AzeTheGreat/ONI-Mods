using HarmonyLib;
using Klei.AI;
using UnityEngine;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Amount), nameof(Amount.Copy))]
    class TriggerAmountCopy
    {
        static void Postfix(GameObject to, GameObject from)
        {
            to.Trigger((int)GameHashes.CopySettings, from);
        }
    }
}
