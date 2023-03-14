using HarmonyLib;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(BuildingConfigManager), nameof(BuildingConfigManager.OnPrefabInit))]
    class Patch_BuildingConfigManager_OnPrefabInit
    {
        static void Postfix(ref GameObject ___baseTemplate)
        {
            ___baseTemplate.AddComponent<BuildingSuppressionButton>();
        }
    }
}
