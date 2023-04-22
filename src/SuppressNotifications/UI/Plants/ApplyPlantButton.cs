using HarmonyLib;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(Uprootable), nameof(Uprootable.OnPrefabInit))]
    class ApplyPlantButton
    {
        static void Postfix(Uprootable __instance)
        {
            __instance.gameObject.AddComponent<PlantSuppressionButton>();
        }
    }
}
