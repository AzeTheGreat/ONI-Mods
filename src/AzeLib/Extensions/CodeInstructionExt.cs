using Harmony;
using System;
using System.Reflection.Emit;

namespace AzeLib.Extensions
{
    public static class CodeInstructionExt
    {
        public static bool IsLocalOfType(this CodeInstruction i, Type type)
        {
            var localBuilder = i.operand as LocalBuilder;
            if (localBuilder != null && localBuilder.LocalType == type)
                return true;
            return false;
        }
    }
}
