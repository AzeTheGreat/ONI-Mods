using Harmony;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(PlanScreen), "OnClickCategory")]
    public class CategoryFix_Patch
    {
        //private static InterfaceTool lastTool;

        //static void Prefix()
        //{
        //    lastTool = PlayerController.Instance.ActiveTool;
        //}

        //static void Postfix()
        //{
        //    PlayerController.Instance.ActivateTool(SelectTool.Instance);
        //    Debug.Log("?");
        //    if (Options.options.Mode == Options.FixMode.Hold)
        //        PlayerController.Instance.ActivateTool(lastTool);
        //}
    }

    //[HarmonyPatch(typeof(PlanScreen), "DeactivateBuildTools")]
    //public class Test
    //{
    //    static bool Prefix()
    //    {
    //        InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
    //        if (activeTool != null)
    //        {
    //            activeTool.DeactivateTool(null);
    //            PlayerController.Instance.ActivateTool(SelectTool.Instance);
    //        }
    //        return false;
    //    }
    //}
}
