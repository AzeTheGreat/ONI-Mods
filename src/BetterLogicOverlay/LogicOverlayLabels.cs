using BetterLogicOverlay.LogicSettingDisplay;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterLogicOverlay
{
    static class LogicOverlayLabels
    {
        private static Dictionary<ILogicUIElement, (LogicSettingUIInfo uiInfo, GameObject go)> logicSettingUIs = new();
        private static UIPool<LocText> uiGOPool;

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.FreeUI))]
        static class Remove
        {
            static void Postfix(ILogicUIElement item)
            {
                if (item == null)
                    return;

                if (logicSettingUIs.TryGetValue(item, out var uiGOPair))
                {
                    logicSettingUIs.Remove(item);
                    uiGOPool.ClearElement(uiGOPair.uiInfo.cachedLocText);
                }
            }
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.AddUI))]
        static class Add
        {
            static void Postfix(ILogicUIElement ui_elem)
            {
                if (ui_elem?.GetGO() is not GameObject go)
                    return;

                if (!logicSettingUIs.Values.Any(x => x.go == go) && go.GetComponent<LogicLabelSetting>() is LogicLabelSetting dispComp)
                {
                    var uiInfo = new LogicSettingUIInfo(uiGOPool.GetFreeElement(GameScreenManager.Instance.worldSpaceCanvas), dispComp);
                    logicSettingUIs.Add(ui_elem, (uiInfo, go));
                }
            }
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.UpdateUI))]
        static class Update
        {
            static void Postfix()
            {
                foreach (var logicSettingUI in logicSettingUIs.Values)
                    logicSettingUI.uiInfo.UpdateText();
            }
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.Disable))]
        static class Disable
        {
            static void Postfix()
            {
                logicSettingUIs.Clear();
                uiGOPool.ClearAll();
            }
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.Enable))]
        static class Enable
        {
            // Must reinitialize or the game will crash after a load.
            static void Postfix()
            {
                uiGOPool = new UIPool<LocText>(LabelPrefab.GetLabelPrefab());
            }
        }
    }
}
