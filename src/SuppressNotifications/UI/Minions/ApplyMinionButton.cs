using HarmonyLib;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(MinionConfig), nameof(MinionConfig.OnPrefabInit))]
    class ApplyMinionButton
    {
        static void Postfix(GameObject go)
        {
            go.AddComponent<MinionSuppressionButton>();
        }
    }
}
