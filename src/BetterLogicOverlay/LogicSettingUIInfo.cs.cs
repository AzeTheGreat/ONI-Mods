using TMPro;
using UnityEngine;

namespace BetterLogicOverlay
{
    class LogicSettingUIInfo
    {
        private GameObject uiGO;
        private TextMeshPro cachedTMP;
        private ILogicSettingDisplay logicSettingDisplay;

        public LogicSettingUIInfo(ILogicUIElement logicPortUI, GameObject prefab)
        {
            GameObject sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.LogicGates];
            if (sourceGO == null)
                sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.Building];

            if (sourceGO == null)
                return;

            ILogicSettingDisplay logicSettingDisplay;
            if ((logicSettingDisplay = sourceGO.GetComponent<ILogicSettingDisplay>()) != null)
                this.logicSettingDisplay = logicSettingDisplay;

            var extents = sourceGO.GetComponent<Building>().GetExtents();
            Vector3 position = Grid.CellToPosCCC(Grid.XYToCell(extents.x, extents.y + extents.height - 1), Grid.SceneLayer.Front) + new Vector3((extents.width - 1) * Grid.CellSizeInMeters / 2, 0f, 0f);
            var offset = new Vector3(0f, (Grid.CellSizeInMeters * 2 / 3) + 0.05f, 0f);
            Vector2 sizeDelta = new Vector2(Grid.CellSizeInMeters * extents.width, Grid.CellSizeInMeters) * prefab.GetComponent<RectTransform>().InverseLocalScale();

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
