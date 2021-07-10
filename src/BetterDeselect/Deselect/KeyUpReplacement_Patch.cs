using HarmonyLib;
using PeterHan.PLib.Options;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnKeyUp))]
    class KeyUpReplacement_Patch
    {
        static bool Prefix(PlanScreen __instance, KButtonEvent e, KIconToggleMenu.ToggleInfo ___activeCategoryInfo)
        {
            bool isActiveMenu = ___activeCategoryInfo != null;
            bool isActiveOverlay = OverlayScreen.Instance.GetMode() != OverlayModes.None.ID;

            // The first click, when something is selected, doesn't get processed here.
            // It goes through ToolMenu and gets processed by PlanScreen.OnActiveToolChanged.

            // If both are active, check if they're supposed to be closed on the second click, and close.
            if (isActiveMenu && isActiveOverlay && TryConsumeRightClick())
            {
                if (Options.Opts.BuildMenu == Options.ClickNum.Two)
                    CloseMenu();
                if (Options.Opts.Overlay == Options.ClickNum.Two)
                    CloseOverlay();
            }
            // If either the menu or overlay is active, close it.
            else if ((isActiveMenu ^ isActiveOverlay) && TryConsumeRightClick())
            {
                if (isActiveMenu)
                    CloseMenu();
                if (isActiveOverlay)
                    CloseOverlay();
            }

            return false;

            bool TryConsumeRightClick() => PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight);
            void CloseMenu() => __instance.OnClickCategory(___activeCategoryInfo);
            void CloseOverlay() => OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
        }
    }
}
