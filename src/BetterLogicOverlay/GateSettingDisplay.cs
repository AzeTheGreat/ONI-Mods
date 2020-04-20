using BetterLogicOverlay.LogicSettingDisplay;
using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;
using UnityEngine;

namespace BetterLogicOverlay
{
    class GateSettingDisplay
    {
        private static GateSettingDisplay instance;
        private GameObject logicSettingUIPrefab;
        private Dictionary<ILogicUIElement, LogicSettingUIInfo> logicSettingUIs = new Dictionary<ILogicUIElement, LogicSettingUIInfo>();
        private HashSet<GameObject> existingGOs = new HashSet<GameObject>();
        private UIGameObjectPool uiPool;
        private GameObject powerCanvas;

        private void OnEnable()
        {
            CreateUIPrefab();
            uiPool = new UIGameObjectPool(logicSettingUIPrefab);
            powerCanvas = Traverse.Create(OverlayScreen.Instance).GetField<Canvas>("powerLabelParent").gameObject;
        }

        private void OnDisable()
        {
            logicSettingUIs.Clear();
            uiPool.ClearAll();
            existingGOs.Clear();
        }

        private void OnAddUI(ILogicUIElement logicPortUI)
        {
            if (!(GetGOForLogicPort(logicPortUI) is GameObject go) || existingGOs.Contains(go))
                return;

            existingGOs.Add(go);
            if (go.GetComponent<LogicSettingDispComp>() is LogicSettingDispComp dispComp)
                logicSettingUIs.Add(logicPortUI, new LogicSettingUIInfo(go, uiPool.GetFreeElement(powerCanvas), dispComp));
            //GameScreenManager.Instance.worldSpaceCanvas
        }

        private void OnFreeUI(ILogicUIElement logicPortUI)
        {
            if (logicPortUI == null)
                return;

            if (logicSettingUIs.TryGetValue(logicPortUI, out LogicSettingUIInfo logicSettingUIInfo))
            {
                logicSettingUIs.Remove(logicPortUI);
                existingGOs.Remove(logicSettingUIInfo.sourceGO);
                uiPool.ClearElement(logicSettingUIInfo.uiGO);
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

        private void CreateUIPrefab()
        {
            //var settingPrefab = new GameObject("LogicSettingPrefab");
            //settingPrefab.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform, false);
            //settingPrefab.layer = 5;

            //var rectTransform = settingPrefab.AddComponent<RectTransform>();
            //rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            //rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            //rectTransform.pivot = new Vector2(0.5f, 0.5f);
            //rectTransform.localScale = Vector2.one / 3f;

            //var tmp = settingPrefab.AddComponent<TextMeshPro>();
            //tmp.fontSize = Options.Opts.FontSize;
            //tmp.characterSpacing = -4f;
            //tmp.lineSpacing = -30f;
            //tmp.fontStyle = FontStyles.Bold;
            //tmp.enableKerning = true;

            ////tmp.fontSharedMaterial.EnableKeyword(ShaderUtilities.Keyword_Underlay);
            ////tmp.fontSharedMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0.4f);
            ////tmp.fontSharedMaterial.SetFloat(ShaderUtilities.ID_UnderlaySoftness, 0.35f);
            //tmp.UpdateMeshPadding();

            //tmp.alignment = TMPro.TextAlignmentOptions.Bottom;
            //tmp.enableWordWrapping = true;
            //tmp.overflowMode = TMPro.TextOverflowModes.Truncate;
            //tmp.raycastTarget = false;

            ////var panel = settingPrefab.AddComponent<Image>();
            ////panel.color = new Color(0f, 0f, 0f, 100f);

            //logicSettingUIPrefab = settingPrefab;

            logicSettingUIPrefab = Traverse.Create(OverlayScreen.Instance).GetField<LocText>("powerLabelPrefab").gameObject;
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
        private class AddUI { static void Postfix(ILogicUIElement ui_elem) => instance.OnAddUI(ui_elem);
            static bool Prepare() => Options.Opts.DisplayLogicSettings; }

        [HarmonyPatch(typeof(OverlayModes.Logic), "FreeUI")]
        private class FreeUI { static void Postfix(ILogicUIElement item) => instance.OnFreeUI(item);
            static bool Prepare() => Options.Opts.DisplayLogicSettings; }

        [HarmonyPatch(typeof(OverlayModes.Logic), "UpdateUI")]
        private class UpdateUI { static void Postfix() => instance.OnUpdateUI();
            static bool Prepare() => Options.Opts.DisplayLogicSettings; }
    }
}
