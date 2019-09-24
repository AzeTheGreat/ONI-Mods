using Harmony;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(PlanScreen), "OnClickCategory")]
    public class HeldCategoryFix_Patch
    {
        static void Postfix()
        {
            if (Options.options.ImplementReselectFix)
                PlayerController.Instance.ActivateTool(SelectTool.Instance);
        }
    }
}
