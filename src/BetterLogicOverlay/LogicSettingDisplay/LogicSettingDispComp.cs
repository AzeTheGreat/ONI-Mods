using UnityEngine;
using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicSettingDispComp : AMonoBehaviour
    {
        public abstract string GetSetting();

        [MyCmpGet] protected Building building;

        public virtual Vector2 GetPosition()
        {
            var extents = building.GetExtents();
            return Grid.CellToPosCCC(Grid.XYToCell(extents.x, extents.y + extents.height - 1), Grid.SceneLayer.Front) + new Vector3((extents.width - 1) * Grid.CellSizeInMeters / 2, 0f, 0f);
        }

        public virtual Vector2 GetSizeDelta()
        {
            var extents = building.GetExtents();
            return new Vector2(Grid.CellSizeInMeters * extents.width, Grid.CellSizeInMeters);
        }
    }
}
