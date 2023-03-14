using HarmonyLib;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Crop), nameof(Crop.OnPrefabInit))]
    class Patch_Crop_OnPrefabInit
    {
        static void Postfix(Crop __instance)
        {
            __instance.gameObject.AddComponent<CropSuppressionButton>();
        }
    }
}
