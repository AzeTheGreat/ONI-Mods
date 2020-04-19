using System.Collections.Generic;
using System.Linq;

namespace AzeLib.Extensions
{
    public static class LogicPortsExt
    {
        public static IEnumerable<int> GetLogicCells(this LogicPorts logicPorts)
        {
            return (logicPorts.inputPorts ?? new List<ILogicUIElement>())
                .Concat(logicPorts.outputPorts ?? new List<ILogicUIElement>())
                .Select(x => x.GetLogicUICell());
        }

        public static IEnumerable<int> GetLogicCells(this LogicGateBase logicGateBase)
        {
            int cell = Grid.PosToCell(logicGateBase);
            var rotatable = logicGateBase.GetComponent<Rotatable>();

            return (logicGateBase.inputPortOffsets ?? new CellOffset[0])
                .Concat(logicGateBase.outputPortOffsets ?? new CellOffset[0])
                .Concat(logicGateBase.controlPortOffsets ?? new CellOffset[0])
                .Select(x => GetActualCell(x));

            int GetActualCell(CellOffset offset)
            {
                if (rotatable)
                    offset = rotatable.GetRotatedCellOffset(offset);
                
                return Grid.OffsetCell(cell, offset);
            }
        }
    }
}
