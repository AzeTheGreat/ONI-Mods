using BetterLogicOverlay.LogicSettingDisplay;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay
{
    static class LogicOverlayLabels
    {
        private static Dictionary<GameObject, LogicSettingUIInfo> logicSettingUIs = new Dictionary<GameObject, LogicSettingUIInfo>();
        private static UIPool<LocText> uiGOPool;

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.FreeUI))]
        static class Remove
        {
            static void Postfix(ILogicUIElement item)
            {
                var go = item.GetGO();
                if (go != null && logicSettingUIs.TryGetValue(go, out LogicSettingUIInfo logicSettingUIInfo))
                {
                    logicSettingUIs.Remove(go);
                    uiGOPool.ClearElement(logicSettingUIInfo.cachedLocText);
                }
            }
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.AddUI))]
        static class Add
        {
            static void Postfix(ILogicUIElement ui_elem)
            {
                var go = ui_elem.GetGO();
                if (go != null && !logicSettingUIs.ContainsKey(go) && go.GetComponent<LogicLabelSetting>() is LogicLabelSetting dispComp)
                    logicSettingUIs.Add(go, new LogicSettingUIInfo(uiGOPool.GetFreeElement(GameScreenManager.Instance.worldSpaceCanvas), dispComp));
            }
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.UpdateUI))]
        static class Update
        {
            static void Postfix()
            {
                foreach (var logicSettingUI in logicSettingUIs.Values)
                    logicSettingUI.UpdateText();
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
