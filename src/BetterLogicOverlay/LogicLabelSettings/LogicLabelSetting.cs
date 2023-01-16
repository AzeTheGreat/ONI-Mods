using AzeLib.Attributes;
using AzeLib.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicLabelSetting : AMonoBehaviour
    {
        [MyCmpGet] private LogicPorts logicPorts;
        [MyCmpGet] private LogicGateBase logicGateBase;

        public Vector2 Position => (Vector2)transform.position + offset;
        
        public Vector2 sizeDelta;

        private Vector2 offset;

        public abstract string GetSetting();

        public bool ContainsLogicCell(int cell) => (logicPorts?.TryGetPortAtCell(cell, out _, out _) ?? false) || (logicGateBase?.TryGetPortAtCell(cell, out _) ?? false);

        public override void OnSpawn()
        {
            base.OnSpawn();

            // Could be optimized if it causes performance issues.
            var longestPorts = GetCelloffsets()
                .GroupBy(x => x.y)
                //.SelectMany(x => x.GroupConsecutive()) only necessary if a mod adds a building with non adjacent ports in a row.
                .LinqByValue((s, r) => s.Where(x => x.Count() == r), s => s.Min(x => x.Count()))
                .LinqByValue((s, r) => s.First(x => x.Key == r) , s => s.Max(x => x.Key))
                .OrderBy(x => x.x);

            float cellSize = Grid.CellSizeInMeters;
            float portSize = Assets.instance.logicModeUIData.prefab.GetComponent<RectTransform>().sizeDelta.y;

            var firstCell = longestPorts.First();
            var lastCell = longestPorts.Last();
            float width = (lastCell.x - firstCell.x + 1) * cellSize;

            // Offset pos to center of logic ports span, just above the port icon.
            offset = firstCell.ToVector2I() + new Vector2((width - cellSize) / 2f, (cellSize + portSize) / 2);
            sizeDelta = new Vector2(width, cellSize - portSize + LabelPrefab.boundsHeightDelta);

            IEnumerable<CellOffset> GetCelloffsets()
            {
                var cellOffsets = (logicPorts?.GetLogicCellOffsets() ?? logicGateBase?.GetLogicCellOffsets());

                var rot = GetComponent<Rotatable>();
                if (rot != null)
                    return cellOffsets.Select(x => rot.GetRotatedCellOffset(x));
                return cellOffsets;
            }
        }
    }
}


