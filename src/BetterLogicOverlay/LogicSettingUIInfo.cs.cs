using TMPro;
using UnityEngine;
using BetterLogicOverlay.LogicSettingDisplay;

namespace BetterLogicOverlay
{
    class LogicSettingUIInfo
    {
        private GameObject uiGO;
        private TextMeshPro cachedTMP;
        private LogicSettingDispComp logicSettingDisplay;

        public LogicSettingUIInfo(ILogicUIElement logicPortUI, GameObject prefab)
        {
            GameObject sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.LogicGates];
            if (sourceGO == null)
                sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.Building];
            if (sourceGO == null)
                sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.FoundationTile];
                
            if (sourceGO == null)
                return;

            LogicSettingDispComp logicSettingDisplay = sourceGO.GetComponent<LogicSettingDispComp>();
            if (logicSettingDisplay == null)
                return;

            this.logicSettingDisplay = logicSettingDisplay;

            Vector3 position = logicSettingDisplay.GetPosition();
            var offset = new Vector3(0f, (Grid.CellSizeInMeters * 2 / 3) + 0.05f, 0f);
            Vector2 sizeDelta = logicSettingDisplay.GetSizeDelta() * prefab.GetComponent<RectTransform>().InverseLocalScale();

            uiGO = DrawSetting(prefab, position + offset, sizeDelta);
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
