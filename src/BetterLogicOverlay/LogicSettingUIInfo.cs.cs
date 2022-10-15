using AzeLib.Extensions;
using BetterLogicOverlay.LogicSettingDisplay;
using UnityEngine;

namespace BetterLogicOverlay
{
    struct LogicSettingUIInfo
    {
        public LocText cachedLocText;
        private LogicLabelSetting logicSettingDisplay;

        public LogicSettingUIInfo(LocText prefab, LogicLabelSetting logicSettingDispComp)
        {
            logicSettingDisplay = logicSettingDispComp;
            cachedLocText = prefab;

            prefab.transform.position = logicSettingDisplay.position + LabelPrefab.boundsYOffset;
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.sizeDelta = logicSettingDisplay.sizeDelta * rectTransform.InverseLocalScale();
            prefab.gameObject.SetActive(true);
        }

        public void UpdateText() => cachedLocText.text = logicSettingDisplay.GetSetting();
    }
}
