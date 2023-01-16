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

            prefab.transform.position = logicSettingDisplay.Position + LabelPrefab.boundsYOffset;

            // Set as last sibling so that labels are always drawn over port icons.
            prefab.transform.SetAsLastSibling();

            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.sizeDelta = logicSettingDisplay.sizeDelta * rectTransform.InverseLocalScale();

            prefab.gameObject.SetActive(true);
        }

        public void UpdateText() => cachedLocText.text = logicSettingDisplay.GetSetting();
    }
}
