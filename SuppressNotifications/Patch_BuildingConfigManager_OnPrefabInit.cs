using Harmony;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(BuildingConfigManager), "OnPrefabInit")]
    class Patch_BuildingConfigManager_OnPrefabInit
    {
        static void Postfix(ref GameObject ___baseTemplate)
        {
            ___baseTemplate.AddComponent<BuildingSuppressionButton>();
        }
    }
}
