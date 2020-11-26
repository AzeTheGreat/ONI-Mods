using BetterLogicOverlay.LogicSettingDisplay;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay
{
    static class LogicOverlayLabels
    {
        private static Dictionary<GameObject, LogicSettingUIInfo> logicSettingUIs = new Dictionary<GameObject, LogicSettingUIInfo>();
        private static UIPool<LocText> uiGOPool;

        [HarmonyPatch()]
        static class Remove
        {
            static MethodBase TargetMethod() => AccessTools.Method(AccessTools.TypeByName("OverlayModes+Logic+<>c__DisplayClass37_0"), "<Update>b__0");

            static void Postfix(SaveLoadRoot root)
            {
                var go = root.gameObject;
                if (logicSettingUIs.TryGetValue(go, out LogicSettingUIInfo logicSettingUIInfo))
                {
                    logicSettingUIs.Remove(go);
                    uiGOPool.ClearElement(logicSettingUIInfo.cachedLocText);
                }
            }
        }

        [HarmonyPatch()]
        static class Add
        {
            static MethodBase TargetMethod() => AccessTools.Method(AccessTools.TypeByName("OverlayModes+Logic+<>c"), "<Update>b__37_4");

            static void Postfix(SaveLoadRoot root)
            {
                var go = root.gameObject;
                if (go.GetComponent<LogicLabelSetting>() is LogicLabelSetting dispComp)
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
