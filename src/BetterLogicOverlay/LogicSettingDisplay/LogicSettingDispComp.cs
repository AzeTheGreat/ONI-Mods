using UnityEngine;
using AzeLib.Attributes;
using AzeLib.Extensions;
using System.Linq;
using LibNoiseDotNet.Graphics.Tools.Noise.Renderer;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    abstract class LogicSettingDispComp : AMonoBehaviour
    {
        public abstract string GetSetting();

        [MyCmpGet] protected Building building;

        internal Vector2 position;
        internal Vector2 sizeDelta;

        protected override void OnSpawn()
        {
            float cellSize = Grid.CellSizeInMeters;
            float portSize = Assets.instance.logicModeUIData.prefab.GetComponent<RectTransform>().sizeDelta.y;

            var longestPorts = GetComponent<LogicPorts>()
                .GetLogicCells()
                .GroupBy(x => Grid.CellToXY(x).y)
                //.SelectMany(x => x.GroupConsecutive()) only necessary if a mod adds a building with non adjacent ports in a row.
                .OrderBy(x => x.Count())
                .FirstOrDefault();

            var firstCell = longestPorts.First();
            var lastCell = longestPorts.Last();
            float width = (lastCell - firstCell + 1) * cellSize;

            position = Grid.CellToPosCTC(firstCell, Grid.SceneLayer.Front)
                + new Vector3((width - cellSize) * cellSize / 2f, 0.01f, 0f);
            sizeDelta = new Vector2(width, cellSize - portSize);

            base.OnSpawn();
        }
    }
}
