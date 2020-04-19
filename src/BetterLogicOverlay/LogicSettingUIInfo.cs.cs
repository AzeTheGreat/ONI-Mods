using TMPro;
using UnityEngine;
using BetterLogicOverlay.LogicSettingDisplay;
using AzeLib.Extensions;

namespace BetterLogicOverlay
{
    class LogicSettingUIInfo
    {
        public GameObject sourceGO;
        private GameObject uiGO;
        private TextMeshPro cachedTMP;
        private LogicSettingDispComp logicSettingDisplay;

        public LogicSettingUIInfo(GameObject sourceGO, GameObject prefab)
        {
            if (sourceGO == null)
                return;

            this.sourceGO = sourceGO;
            LogicSettingDispComp logicSettingDisplay = sourceGO.GetComponent<LogicSettingDispComp>();
            if (logicSettingDisplay == null)
                return;

            this.logicSettingDisplay = logicSettingDisplay;

            Vector2 sizeDelta = logicSettingDisplay.sizeDelta * prefab.GetComponent<RectTransform>().InverseLocalScale();

            uiGO = DrawSetting(prefab, logicSettingDisplay.position, sizeDelta);
            cachedTMP = uiGO.GetComponent<TextMeshPro>();
        }

        public void Destroy()
        {
            if(uiGO != null)
                Util.KDestroyGameObject(uiGO);
        }

        public void UpdateText()
        {
            if (logicSettingDisplay != null)
                cachedTMP.text = logicSettingDisplay.GetSetting();        
        }

        private GameObject DrawSetting(GameObject prefab, Vector3 position, Vector2 size)
        {
            GameObject logicSettingUI = Util.KInstantiate(
                prefab,
                position,
                Quaternion.identity,
                GameScreenManager.Instance.worldSpaceCanvas);

            // No clue why I have to set this again
            var rect = logicSettingUI.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            logicSettingUI.SetActive(true);

            return logicSettingUI;
        }
    }
}
