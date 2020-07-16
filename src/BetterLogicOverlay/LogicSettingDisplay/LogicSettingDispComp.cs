using AzeLib.Attributes;
using AzeLib.Extensions;
using System.Linq;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicSettingDispComp : AMonoBehaviour
    {
        public abstract string GetSetting();

        public Vector2 position;
        public Vector2 sizeDelta;

        protected override void OnSpawn()
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

            position = Grid.CellToPosCTC(firstCell, Grid.SceneLayer.Front)              // First cell to pos.
                + new Vector3((width - cellSize) * cellSize / 2f, portSize / 2f, 0f);   // Offset pos to center of span, just above the port.    
            sizeDelta = new Vector2(width, cellSize - portSize);
        }
    }
}


