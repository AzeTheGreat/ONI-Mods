using AzeLib.Extensions;
using STRINGS;
using System;
using System.Linq;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicClusterLocationSensorSetting : LogicLabelSetting
    {
        [MyCmpGet] private LogicClusterLocationSensor sensor;

        public override string GetSetting()
        {
            var count = sensor.activeLocations.Count + Convert.ToInt32(sensor.activeInSpace);
            if (count == 0)
                return UI.CODEX.SUBWORLDS.NONE;

            string clusterName = string.Empty;
            if (sensor.activeInSpace)
                clusterName = UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.EMPTY_SPACE_ROW;
            else if (sensor.activeLocations.FirstOrDefault() is AxialI firstLoc)
                clusterName = ClusterGrid.Instance.GetAsteroidAtCell(firstLoc).GetProperName();
            
            if (count > 1)
                return clusterName.Truncate(5) + " +" + (count - 1);
            return clusterName;
        }
    }
}
