using BetterLogicOverlay.LogicSettingDisplay;
using Harmony;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BetterLogicOverlay
{
    class LogicOverlayLabels
    {
        private static LogicOverlayLabels instance;
        private Dictionary<ILogicUIElement, LogicSettingUIInfo> logicSettingUIs = new Dictionary<ILogicUIElement, LogicSettingUIInfo>();
        private HashSet<GameObject> existingGOs = new HashSet<GameObject>();
        private UIPool<LocText> uiGOPool;

        public static void OnLoad() => instance = new LogicOverlayLabels();
        private void OnEnable() => uiGOPool = new UIPool<LocText>(CreateUIPrefab());

        private void OnDisable()
        {
            logicSettingUIs.Clear();
            uiGOPool.ClearAll();
            existingGOs.Clear();
        }

        private void OnAddUI(ILogicUIElement logicPortUI)
        {
            if (!(GetGOForLogicPort(logicPortUI) is GameObject go) || existingGOs.Contains(go))
                return;

            existingGOs.Add(go);
            if (go.GetComponent<LogicLabelSetting>() is LogicLabelSetting dispComp)
                logicSettingUIs.Add(logicPortUI, new LogicSettingUIInfo(go, uiGOPool.GetFreeElement(GameScreenManager.Instance.worldSpaceCanvas), dispComp));
        }

        private void OnFreeUI(ILogicUIElement logicPortUI)
        {
            if (logicPortUI == null)
                return;

            if (logicSettingUIs.TryGetValue(logicPortUI, out LogicSettingUIInfo logicSettingUIInfo))
            {
                logicSettingUIs.Remove(logicPortUI);
                existingGOs.Remove(logicSettingUIInfo.sourceGO);
                uiGOPool.ClearElement(logicSettingUIInfo.cachedLocText);
            }
        }

        private GameObject GetGOForLogicPort(ILogicUIElement logicPortUI)
        {
            int cell = logicPortUI.GetLogicUICell();
            return Grid.Objects[cell, (int)ObjectLayer.LogicGate] ??
                Grid.Objects[cell, (int)ObjectLayer.Building] ??
                Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
        }

        private void OnUpdateUI()
        {
            foreach (var logicSettingUI in logicSettingUIs.Values)
                logicSettingUI.UpdateText();
        }

        private LocText CreateUIPrefab()
        {
            // Create custom prefab, using the power label prefab as the base.
            var prefab = Object.Instantiate(OverlayScreen.Instance.powerLabelPrefab);
            prefab.name = "LogicSettingLabel";

            Object.Destroy(prefab.GetComponent<ContentSizeFitter>());   // Remove fitter so that the rect doesn't expand (it needs to be limited to a cell).
            Object.Destroy(prefab.GetComponent<ToolTip>());             // Unnecessary for this label.
            Object.Destroy(prefab.transform.GetChild(0).gameObject);    // Remove child because a separate GO doesn't work well for limiting bounds.

            prefab.alignment = TextAlignmentOptions.Bottom;
            prefab.enableWordWrapping = true;
            prefab.raycastTarget = false;

            // Adjust font sizing.
            prefab.fontSize -= 1f;
            prefab.characterSpacing = -3f;
            prefab.lineSpacing = -15f;

            return prefab;
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.Enable))]
        private class Enable_Patch { static void Postfix() => instance.OnEnable(); }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.Disable))]
        private class Disable { static void Postfix() => instance.OnDisable(); }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.AddUI))]
        private class AddUI {
            static bool Prepare() => Options.Opts.DisplayLogicSettings;
            static void Postfix(ILogicUIElement ui_elem) => instance.OnAddUI(ui_elem); }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.FreeUI))]
        private class FreeUI {
            static bool Prepare() => Options.Opts.DisplayLogicSettings;
            static void Postfix(ILogicUIElement item) => instance.OnFreeUI(item); }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.UpdateUI))]
        private class UpdateUI {
            static bool Prepare() => Options.Opts.DisplayLogicSettings;
            static void Postfix() => instance.OnUpdateUI(); }
    }
}
