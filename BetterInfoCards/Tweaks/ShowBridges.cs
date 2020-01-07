
using Harmony;

namespace BetterInfoCards.Tweaks
{
    // By default the game doesn't show bridges in the gas or liquid overlays, but then you can't select them due to hit modification
    // This fixes both issues

    [HarmonyPatch(typeof(SelectToolHoverTextCard), "ShouldShowGasConduitOverlay")]
    class ShowGasBridges
    {
        static void Postfix(ref bool __result, KSelectable selectable)
        {
            var component = selectable.GetComponent<ConduitBridge>();
            __result = __result || (component != null && component.type == ConduitType.Gas);
        }
    }

    [HarmonyPatch(typeof(SelectToolHoverTextCard), "ShouldShowLiquidConduitOverlay")]
    class ShowLiquidBridges
    {
        static void Postfix(ref bool __result, KSelectable selectable)
        {
            var component = selectable.GetComponent<ConduitBridge>();
            __result = __result || (component != null && component.type == ConduitType.Liquid);
        }
    }
}
