using System;
using TMPro;
using UnityEngine;

namespace BetterLogicPortDisplay
{
    class LogicSettingUIInfo
    {
        private GameObject uiGO;
        private TextMeshPro cachedTMP;
        private object cachedObject;
        private Func<object, string> getObjectString;

        public LogicSettingUIInfo(ILogicUIElement logicPortUI, GameObject prefab)
        {
            GameObject sourceGO;
            if ((sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.LogicGates]) != null)
            {
                ISliderControl sliderControl;
                if ((sliderControl = sourceGO.GetComponent<ISliderControl>()) != null)
                {
                    cachedObject = sliderControl;
                    getObjectString = (obj) =>
                    {
                        var component = obj as ISliderControl;
                        return component.GetSliderValue(0) + component.SliderUnits;
                    };
                }
            }
            else if ((sourceGO = Grid.Objects[logicPortUI.GetLogicUICell(), (int)ObjectLayer.Building]) != null)
            {
                IThresholdSwitch thresholdSwitch;
                if ((thresholdSwitch = sourceGO.GetComponent<IThresholdSwitch>()) != null)
                {
                    cachedObject = thresholdSwitch;
                    getObjectString = (obj) =>
                    {
                        var component = obj as IThresholdSwitch;
                        string aboveOrBelow = component.ActivateAboveThreshold ? ">" : "<";
                        return aboveOrBelow + GameUtil.GetFormattedMass(component.Threshold);
                    };
                }
            }
            else
                return;

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
            if(cachedObject != null)
            {
                string newText = getObjectString(cachedObject);
                if(cachedTMP.text != newText)
                    cachedTMP.text = newText;
            }
                    
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
