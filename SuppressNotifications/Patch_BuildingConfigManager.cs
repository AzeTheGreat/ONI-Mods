using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(BuildingConfigManager), "OnPrefabInit")]
    class Patch_BuildingConfigManager
    {
        static void Postfix(ref GameObject ___baseTemplate)
        {
            ___baseTemplate.AddComponent<BuildingNotificationButton>();
        }
    }
}
