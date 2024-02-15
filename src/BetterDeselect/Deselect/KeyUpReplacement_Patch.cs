using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnKeyUp))]
    class KeyUpReplacement_Patch
    {
        static bool Prefix(PlanScreen __instance, KButtonEvent e, KIconToggleMenu.ToggleInfo ___activeCategoryInfo)
        {
            var activeUIs = new List<(Options.ClickNum clickNum, bool isActive, System.Action close)>()
            {
                (Options.Opts.SelectedObj,
                SelectTool.Instance.selected != null,
                CloseSelectedObjMenu),

                (Options.Opts.BuildMenu,
                ___activeCategoryInfo != null,
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

            return false;

            bool TryConsumeRightClick() => PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight);
            void CloseSelectedObjMenu() => SelectTool.Instance.Select(null);
            void CloseMenu() => __instance.OnClickCategory(___activeCategoryInfo);
            void CloseOverlay() => OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
        }
    }
}
