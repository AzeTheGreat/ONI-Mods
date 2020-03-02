using Harmony;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.DeactivateTool))]
    public class DeselectOverlay_Patch
    {
        static bool Prepare() => Options.Opts.ImplementOverlay;

        static void Prefix(ref HashedString ___toolActivatedViewMode)
        {
            ___toolActivatedViewMode = OverlayModes.None.ID;
        }
    }
}
