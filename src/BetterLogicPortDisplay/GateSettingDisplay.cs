using Harmony;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BetterLogicPortDisplay
{
    class GateSettingDisplay
    {
        private static GateSettingDisplay instance;
        private GameObject logicSettingUIPrefab;
        private Dictionary<ILogicUIElement, LogicSettingUIInfo> logicSettingUIs = new Dictionary<ILogicUIElement, LogicSettingUIInfo>();

        private void OnEnable()
        {
            CreateUIPrefab();
        }

        private void OnDisable()
        {
            foreach (var logicSettingUIInfo in logicSettingUIs.Values)
            {
                logicSettingUIInfo.Destroy();
            }

            logicSettingUIs.Clear();
        }

        private void OnAddUI(ILogicUIElement logicPortUI)
        {
            if(logicPortUI.GetLogicPortSpriteType() == LogicPortSpriteType.Output)
            {
                if (!logicSettingUIs.ContainsKey(logicPortUI))
                    logicSettingUIs.Add(logicPortUI, new LogicSettingUIInfo(logicPortUI, logicSettingUIPrefab));
            }
        }

        private void OnFreeUI(ILogicUIElement logicPortUI)
        {
            if (logicPortUI != null && logicSettingUIs.TryGetValue(logicPortUI, out LogicSettingUIInfo logicSettingUIInfo))
            {
                logicSettingUIs.Remove(logicPortUI);
                logicSettingUIInfo.Destroy();
            }
        }

        private void OnUpdateUI()
        {
            foreach (var logicSettingUI in logicSettingUIs.Values)
            {
                logicSettingUI.UpdateText();
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
            rectTransform.localScale = Vector2.one / 3f;

            var tmp = settingPrefab.AddComponent<TextMeshPro>();
            tmp.fontSize = 9f;
            tmp.characterSpacing = -4f;
            tmp.lineSpacing = -30f;
            tmp.fontStyle = FontStyles.Bold;

            tmp.fontSharedMaterial.EnableKeyword(ShaderUtilities.Keyword_Underlay);
            tmp.fontSharedMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0.5f);
            tmp.fontSharedMaterial.SetFloat(ShaderUtilities.ID_UnderlaySoftness, 0.35f);
            //tmp.fontSharedMaterial.EnableKeyword(ShaderUtilities.Keyword_Outline);
            //tmp.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.15f);
            tmp.UpdateMeshPadding();

            tmp.alignment = TMPro.TextAlignmentOptions.Bottom;
            tmp.enableWordWrapping = true;
            tmp.overflowMode = TMPro.TextOverflowModes.Truncate;
            tmp.raycastTarget = false;

            logicSettingUIPrefab = settingPrefab;
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
