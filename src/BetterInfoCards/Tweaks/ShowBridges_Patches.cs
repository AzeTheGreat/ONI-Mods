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
            if (selectable.GetComponent<ConduitBridge>() is ConduitBridge bridge)
                __result |= bridge.type == ConduitType.Gas;
        }
    }

    [HarmonyPatch(typeof(SelectToolHoverTextCard), "ShouldShowLiquidConduitOverlay")]
    class ShowLiquidBridges
    {
        static void Postfix(ref bool __result, KSelectable selectable)
        {
            if (selectable.GetComponent<ConduitBridge>() is ConduitBridge bridge)
                __result |= bridge.type == ConduitType.Liquid;
        }
    }
}