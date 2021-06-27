using HarmonyLib;
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

        //public static bool Is(this CodeInstruction i, OpCode opCode, object operand)
        //{
        //    return i.OpCodeIs(opCode) && i.OperandIs(operand);
        //}

        //public static bool OperandIs(this CodeInstruction i, object operand)
        //{
        //    return i.operand == operand;
        //}

        public static bool OpCodeIs(this CodeInstruction i, OpCode opCode)
        {
            return i.opcode == opCode;
        }
    }
}
