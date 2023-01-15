using System.Collections.Generic;
using System.Linq;

namespace AzeLib.Extensions
{
    public static class LogicPortsExt
    {
        public static IEnumerable<int> GetLogicCells(this LogicPorts logicPorts)
        {
            return (logicPorts.inputPortInfo.EmptyIfNull())
                .Concat(logicPorts.outputPortInfo.EmptyIfNull())
                .Select(x => logicPorts.GetActualCell(x.cellOffset));
        }

        public static IEnumerable<int> GetLogicCells(this LogicGateBase logicGateBase)
        {
            return (logicGateBase.inputPortOffsets.EmptyIfNull())
                .Concat(logicGateBase.outputPortOffsets.EmptyIfNull())
                .Concat(logicGateBase.controlPortOffsets.EmptyIfNull())
                .Select(x => logicGateBase.GetActualCell(x));
        }
    }
}
