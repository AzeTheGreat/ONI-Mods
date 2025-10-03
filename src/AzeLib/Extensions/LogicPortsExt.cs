using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AzeLib.Extensions
{
    public static class LogicPortsExt
    {
        public static IEnumerable<CellOffset> GetLogicCellOffsets(this LogicPorts logicPorts)
        {
            return (logicPorts.inputPortInfo.EmptyIfNull())
                .Concat(logicPorts.outputPortInfo.EmptyIfNull())
                .Select(x => x.cellOffset);
        }

        public static IEnumerable<CellOffset> GetLogicCellOffsets(this LogicGateBase logicGateBase)
        {
            return (logicGateBase.inputPortOffsets.EmptyIfNull())
                .Concat(logicGateBase.outputPortOffsets.EmptyIfNull())
                .Concat(logicGateBase.controlPortOffsets.EmptyIfNull());
        }
    }
}
