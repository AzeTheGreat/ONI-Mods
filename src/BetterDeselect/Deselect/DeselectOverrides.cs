using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterDeselect
{
    [HarmonyPatch(typeof(ToolMenu), nameof(ToolMenu.OnKeyDown))]
    class EscapeCloseAll
    {
        static void Prefix(KButtonEvent e)
        {
            if (e.Consumed)
                return;

            if (UIs.ActiveUIs.Any() && e.TryConsume(Action.Escape))
                UIs.ActiveUIs.ForEach(x => x.close());
        }
    }

    [HarmonyPatch(typeof(ToolMenu), nameof(ToolMenu.OnKeyUp))]
    class DeselectOverrides
    {
        static void Prefix(KButtonEvent e)
        {
            if (e.Consumed)
                return;

            if (UIs.ActiveUIs.Any() && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
            {
                var lowestClickNum = UIs.ActiveUIs.Min(pair => pair.clickNum);
                UIs.ActiveUIs
                    .Where(x => x.clickNum == lowestClickNum)
                    .Do(x => x.close());
            }

            return;
        }
    }

    class UIs
    {
        public static List<(Options.ClickNum clickNum, Func<bool> isActive, System.Action close)> ActiveUIs => 
            uis.Where(x => x.isActive()).ToList();

        static List<(Options.ClickNum clickNum, Func<bool> isActive, System.Action close)> uis =
        [
            (Options.Opts.SelectedObj,
            () => !PlayerController.Instance.IsUsingDefaultTool(),
            () => {
                PlayerController.Instance.ActivateTool(SelectTool.Instance);
                ToolMenu.Instance.ClearSelection();

                string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
                if (sound != null)
                    KMonoBehaviour.PlaySound(sound);
            }),

            (Options.Opts.BuildMenu,
            () => PlanScreen.Instance.ActiveCategoryToggleInfo != null,
            () => PlanScreen.Instance.CloseCategoryPanel()),

            (Options.Opts.Overlay,
            () => OverlayScreen.Instance.GetMode() != OverlayModes.None.ID,
            () => OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true))
        ];
    }
}