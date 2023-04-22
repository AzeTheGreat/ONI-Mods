using HarmonyLib;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Uprootable), nameof(Uprootable.OnPrefabInit))]
    class ApplyCropButton
    {
        static void Postfix(Uprootable __instance)
        {
            __instance.gameObject.AddComponent<CropSuppressionButton>();
        }
    }
}
