using Harmony;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(BaseUtilityBuildTool), "OnDeactivateTool")]
    public class BaseUtilityBuildToolFix_Patch
    {
        static void Prefix(InterfaceTool new_tool)
        {
            SelectTool.Instance.Activate();
        }
    }
}
