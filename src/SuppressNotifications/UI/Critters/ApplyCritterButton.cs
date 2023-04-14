using HarmonyLib;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(Capturable), nameof(Capturable.OnPrefabInit))]
    class ApplyCritterButton
    {
        static void Postfix(Capturable __instance)
        {
            __instance.gameObject.AddComponent<CritterSuppressionButton>();
        }
    }
}
