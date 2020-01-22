using UnityEngine;
using AzeLib.Extensions;
using System.Linq;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicSettingDispComp : KMonoBehaviour
    {
        public abstract string GetSetting();

        [MyCmpGet] protected LogicPorts logicPorts;
        [MyCmpGet] protected Building building;

        public Vector2 GetPosition()
        {
            var extents = building.GetExtents();
            if(logicPorts)
                return Grid.CellToPosCCC(logicPorts.GetLogicCells().First(), Grid.SceneLayer.Front);
            else
                return Grid.CellToPosCCC(Grid.XYToCell(extents.x, extents.y + extents.height - 1), Grid.SceneLayer.Front) + new Vector3((extents.width - 1) * Grid.CellSizeInMeters / 2, 0f, 0f);
        }

        public Vector2 GetSizeDelta()
        {
            var extents = building.GetExtents();
            if (logicPorts)
                return Vector2.one * Grid.CellSizeInMeters;
            else
                return new Vector2(Grid.CellSizeInMeters * extents.width, Grid.CellSizeInMeters);
        }
    }
}
