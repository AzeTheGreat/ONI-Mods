using AzeLib;
using HarmonyLib;
using SuppressNotifications;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    [HarmonyPatch(typeof(Assets), nameof(Assets.CreatePrefabs))]
    class ApplySettingsToDefs
    {
        static void Postfix()
        {
            var cmpMap = new ComponentMapper(new()
            {
                (typeof(Uprootable), typeof(EntitySuppressionButton)),
                (typeof(MinionBrain), typeof(EntitySuppressionButton)),
                (typeof(Capturable), typeof(CritterSuppressionButton)),
                (typeof(BuildingComplete), typeof(BuildingSuppressionButton))
            });

            foreach (var prefab in Assets.Prefabs)
                cmpMap.ApplyMap(prefab.gameObject);
        }
    }
}
