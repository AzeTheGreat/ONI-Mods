using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(ToolMenu), nameof(ToolMenu.OnKeyUp))]
    class SetCloseOrder
    {
        static void Prefix(KButtonEvent e)
        {
            if (e.Consumed)
                return;

            var activeUIs = new List<(Options.ClickNum clickNum, bool isActive, System.Action close)>()
            {
                (Options.Opts.SelectedObj,
                !PlayerController.Instance.IsUsingDefaultTool(),
                CloseSelectedObjMenu),

                (Options.Opts.BuildMenu,
                PlanScreen.Instance.ActiveCategoryToggleInfo != null,
                CloseMenu),

                (Options.Opts.Overlay,
                OverlayScreen.Instance.GetMode() != OverlayModes.None.ID,
                CloseOverlay)
            }
            .Where(x => x.isActive);

            if (activeUIs.Any() && TryConsumeRightClick())
            {
                var lowestClickNum = activeUIs.Min(pair => pair.clickNum);
                var uisToClose = activeUIs.Where(pair => pair.clickNum == lowestClickNum);
                foreach (var ui in uisToClose)
                    ui.close();
            }

            return;

            bool TryConsumeRightClick() => PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight);
            void CloseSelectedObjMenu() => PlayerController.Instance.ToolDeactivated(PlayerController.Instance.ActiveTool);
            void CloseMenu() => PlanScreen.Instance.CloseCategoryPanel();
            void CloseOverlay() => OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
        }
    }
}