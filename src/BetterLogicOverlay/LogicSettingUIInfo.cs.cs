using AzeLib.Extensions;
using BetterLogicOverlay.LogicSettingDisplay;
using UnityEngine;

namespace BetterLogicOverlay
{
    struct LogicSettingUIInfo
    {
        public GameObject sourceGO;
        public LocText cachedLocText;
        private LogicSettingDispComp logicSettingDisplay;

        public LogicSettingUIInfo(GameObject sourceGO, LocText prefab, LogicSettingDispComp logicSettingDispComp)
        {
            this.sourceGO = sourceGO;
            logicSettingDisplay = logicSettingDispComp;
            cachedLocText = prefab;

            prefab.gameObject.transform.position = logicSettingDisplay.position;
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.sizeDelta = logicSettingDisplay.sizeDelta * rectTransform.InverseLocalScale();
            prefab.gameObject.SetActive(true);
        }

        public void UpdateText() => cachedLocText.text = logicSettingDisplay.GetSetting();
    }
}
