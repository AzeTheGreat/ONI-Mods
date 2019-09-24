using Harmony;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.DeactivateTool))]
    public class DeselectOverlay_Patch
    {
        static void Prefix(ref HashedString ___toolActivatedViewMode)
        {
            if (Options.options.ImplementOverlay)
                ___toolActivatedViewMode = OverlayModes.None.ID;
        }
    }
}
