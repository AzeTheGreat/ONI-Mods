using AzeLib.Attributes;
using AzeLib.Extensions;
using System.Linq;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicLabelSetting : AMonoBehaviour
    {
        public abstract string GetSetting();

        public Vector2 position;
        public Vector2 sizeDelta;

        public override void OnSpawn()
        {
            base.OnSpawn();

            // Could be optimized if it causes performance issues.
            var longestPorts = (GetComponent<LogicPorts>()?.GetLogicCells() ?? GetComponent<LogicGateBase>()?.GetLogicCells())
                .GroupBy(x => Grid.CellToXY(x).y)
                //.SelectMany(x => x.GroupConsecutive()) only necessary if a mod adds a building with non adjacent ports in a row.
                .LinqByValue((s, r) => s.Where(x => x.Count() == r), s => s.Min(x => x.Count()))
                .LinqByValue((s, r) => s.First(x => x.Key == r) , s => s.Max(x => x.Key))
                .OrderBy(x => x);

            float cellSize = Grid.CellSizeInMeters;
            float portSize = Assets.instance.logicModeUIData.prefab.GetComponent<RectTransform>().sizeDelta.y;

            var firstCell = longestPorts.First();
            var lastCell = longestPorts.Last();
            float width = (lastCell - firstCell + 1) * cellSize;

            // Offset pos to center of logic ports span, just above the port icon.
            position = Grid.CellToPosCTC(firstCell, Grid.SceneLayer.Front)
                + new Vector3((width - cellSize) / 2f, -(cellSize - portSize) / 2, 0f);
            sizeDelta = new Vector2(width, cellSize - portSize + LabelPrefab.boundsHeightDelta);
        }
    }
}


