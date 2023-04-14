using HarmonyLib;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Crop), nameof(Crop.OnPrefabInit))]
    class ApplyCropButton
    {
        static void Postfix(Crop __instance)
        {
            __instance.gameObject.AddComponent<CropSuppressionButton>();
        }
    }
}
