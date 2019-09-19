using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Crop), "OnPrefabInit")]
    class Patch_Crop_OnPrefabInit
    {
        static void Postfix(Crop __instance)
        {
            __instance.gameObject.AddComponent<CropSuppressionButton>();
        }
    }
}
