using Harmony;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(PlanScreen), "OnClickCategory")]
    public class CategoryFix_Patch
    {
        private static InterfaceTool lastTool;

        static void Prefix()
        {
            lastTool = PlayerController.Instance.ActiveTool;
        }

        static void Postfix()
        {
            PlayerController.Instance.ActivateTool(SelectTool.Instance);

            if (Options.options.Mode == Options.FixMode.Hold)
                PlayerController.Instance.ActivateTool(lastTool);
        }
    }
}
