using HarmonyLib;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(Capturable), nameof(Capturable.OnPrefabInit))]
    class Patch_Capturable_OnPrefabInit
    {
        static void Postfix(Capturable __instance)
        {
            __instance.gameObject.AddComponent<CritterSuppressionButton>();
        }
    }
}
