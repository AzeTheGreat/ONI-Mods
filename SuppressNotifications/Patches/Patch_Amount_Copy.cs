using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using Klei.AI;
using UnityEngine;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Amount), nameof(Amount.Copy))]
    class Patch_Amount_Copy
    {
        static void Postfix(GameObject to, GameObject from)
        {
            to.Trigger((int)GameHashes.CopySettings, from);
        }
    }
}
