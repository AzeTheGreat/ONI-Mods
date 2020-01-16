using Harmony;
using PeterHan.PLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace BetterLogicPortDisplay
{
    class GateSettingDisplay
    {
        private static GateSettingDisplay instance;
        private GameObject logicSettingUIPrefab;
        private Dictionary<GameObject, GameObject> logicSettingUIs = new Dictionary<GameObject, GameObject>();

        private void OnEnable()
        {
            CreateUIPrefab();
        }

        private void OnDisable()
        {
            logicSettingUIs.Clear();
        }

        private void OnAddUI(ILogicUIElement logicPortUI)
        {
            if (logicPortUI == null)
                return;

            var go = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.LogicGates];
            if (go == null)
                go = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.Building];

            if (!logicSettingUIs.ContainsKey(go))
                logicSettingUIs.Add(go, DrawSetting(logicPortUI));
        }

        private void OnFreeUI(ILogicUIElement logicPortUI)
        {
            if (logicPortUI == null)
                return;

            var go = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.LogicGates];
            if (go == null)
                go = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.Building];

            if (logicSettingUIs.TryGetValue(go, out GameObject uiInfo))
            {
                logicSettingUIs.Remove(go);
                UnityEngine.Object.Destroy(uiInfo);
            }
        }

        private void OnUpdateUI()
        {
            foreach (var uiInfo in logicSettingUIs)
            {
                var slider = uiInfo.Key.GetComponent<ISliderControl>();
                if (slider != null)
                    uiInfo.Value.GetComponent<TextMeshPro>().text = slider.GetSliderValue(0).ToString();
                var switchSlider = uiInfo.Key.GetComponent<IThresholdSwitch>();
                if (switchSlider != null)
                    uiInfo.Value.GetComponent<TextMeshPro>().text = switchSlider.Threshold.ToString();
            }
        }

        private void CreateUIPrefab()
        {
            var settingPrefab = new GameObject("LogicSettingPrefab");
            settingPrefab.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform, false);
            settingPrefab.layer = 5;

            var rectTransform = settingPrefab.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localScale = Vector2.one;
            rectTransform.sizeDelta = new Vector2(Grid.CellSizeInMeters, Grid.CellSizeInMeters);

            var tmp = settingPrefab.AddComponent<TextMeshPro>();
            tmp.fontSize = 3f;
            tmp.alignment = TMPro.TextAlignmentOptions.Bottom;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TMPro.TextOverflowModes.Overflow;
            tmp.raycastTarget = false;

            logicSettingUIPrefab = settingPrefab;
        }

        private GameObject DrawSetting(ILogicUIElement ui_elem)
        {
            GameObject uiText = global::Util.KInstantiate(
                logicSettingUIPrefab,
                Grid.CellToPosCCC(ui_elem.GetLogicUICell(), Grid.SceneLayer.Front),
                Quaternion.identity,
                GameScreenManager.Instance.worldSpaceCanvas,
                null, true, 0);

            // No clue why I have to set this again
            var rect = uiText.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(Grid.CellSizeInMeters, Grid.CellSizeInMeters);
            uiText.SetActive(true);

            return uiText;
        }

        private static void UpdateBuffer(GameObject uiText, LogicGateBuffer buffer)
        {
            uiText.GetComponent<TextMeshPro>().text = buffer.DelayAmount.ToString();
        }

        public static void OnLoad()
        {
            instance = new GateSettingDisplay();
        }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.Enable))]
        private class Enable_Patch { static void Postfix() => instance.OnEnable(); }

        [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.Disable))]
        private class Disable { static void Postfix() => instance.OnDisable(); }

        [HarmonyPatch(typeof(OverlayModes.Logic), "AddUI")]
        private class AddUI { static void Postfix(ILogicUIElement ui_elem) => instance.OnAddUI(ui_elem); }

        [HarmonyPatch(typeof(OverlayModes.Logic), "FreeUI")]
        private class FreeUI { static void Postfix(ILogicUIElement item) => instance.OnFreeUI(item); }

        [HarmonyPatch(typeof(OverlayModes.Logic), "UpdateUI")]
        private class UpdateUI { static void Postfix() => instance.OnUpdateUI(); }
    }
}
