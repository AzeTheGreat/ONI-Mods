using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicSettingDispComp : KMonoBehaviour
    {
        public abstract string GetSetting();

        public virtual Vector2 GetPosition()
        {
            var extents = gameObject.GetComponent<Building>().GetExtents();
            return Grid.CellToPosCCC(Grid.XYToCell(extents.x, extents.y + extents.height - 1), Grid.SceneLayer.Front) + new Vector3((extents.width - 1) * Grid.CellSizeInMeters / 2, 0f, 0f);
        }

        public virtual Vector2 GetSizeDelta()
        {
            var extents = gameObject.GetComponent<Building>().GetExtents();
            return new Vector2(Grid.CellSizeInMeters * extents.width, Grid.CellSizeInMeters);
        }
    }
}
