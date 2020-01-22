using System.Collections.Generic;

namespace AzeLib.Extensions
{
    public static class LogicPortsExt
    {
        public static List<int> GetLogicCells(this LogicPorts logicPorts)
        {
            var cells = new List<int>();

            foreach (var iPort in logicPorts.inputPorts)
            {
                cells.Add(iPort.GetLogicUICell());
            }

            foreach (var oPort in logicPorts.outputPorts)
            {
                cells.Add(oPort.GetLogicUICell());
            }

            return cells;
        }
    }
}
