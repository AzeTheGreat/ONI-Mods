using BetterLogicOverlay.LogicSettingDisplay;
using UnityEngine;

namespace BetterLogicOverlay
{
    struct LogicSettingUIInfo
    {
        public GameObject sourceGO;
        public GameObject uiGO;
        private LocText cachedTMP;
        private LogicSettingDispComp logicSettingDisplay;

        public LogicSettingUIInfo(GameObject sourceGO, GameObject prefab, LogicSettingDispComp logicSettingDispComp)
        {
            this.sourceGO = sourceGO;
            logicSettingDisplay = logicSettingDispComp;
            uiGO = prefab;
            cachedTMP = prefab.GetComponent<LocText>();

            uiGO.transform.position = logicSettingDisplay.position;
            // No clue why I have to set this again
            //uiGO.GetComponent<RectTransform>().sizeDelta = logicSettingDisplay.sizeDelta * prefab.GetComponent<RectTransform>().InverseLocalScale();
            uiGO.SetActive(true);
        }

        public void UpdateText() => cachedTMP.text = logicSettingDisplay.GetSetting();
    }
}
