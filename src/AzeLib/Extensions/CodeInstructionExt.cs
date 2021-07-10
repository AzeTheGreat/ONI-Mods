using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AzeLib.Extensions
{
    // TODO: Document extensions?
    public static class CodeInstructionExt
    {
        public static bool IsLocalOfType(this CodeInstruction i, Type type)
        {
            if (i.operand is LocalBuilder localBuilder && localBuilder.LocalType == type)
                return true;
            return false;
        }

        public static bool OpCodeIs(this CodeInstruction i, OpCode opCode) => i.opcode == opCode;

        // TODO: This is ugly, make better.  Also expand it to work with everything.
        public static CodeInstruction GetLoadFromStore(this CodeInstruction i)
        {
            var opCode = OpCodes.Ldloc;
            if (i.OpCodeIs(OpCodes.Stloc_0))
                opCode = OpCodes.Ldloc_0;
            if (i.OpCodeIs(OpCodes.Stloc_1))
                opCode = OpCodes.Ldloc_1;
            if (i.OpCodeIs(OpCodes.Stloc_2))
                opCode = OpCodes.Ldloc_2;
            if (i.OpCodeIs(OpCodes.Stloc_3))
                opCode = OpCodes.Ldloc_3;
            if (i.OpCodeIs(OpCodes.Stloc_S))
                opCode = OpCodes.Ldloc_S;

            return new CodeInstruction(opCode, i.operand);
        }

        public static CodeInstruction MakeNop(this CodeInstruction i)
        {
            i.opcode = OpCodes.Nop;
            return i;
        }

        public static CodeInstruction FindNext(this CodeInstruction i, IEnumerable<CodeInstruction> codes, Func<CodeInstruction, bool> predicate) => codes.FindNext(i, predicate);
        public static CodeInstruction FindPrior(this CodeInstruction i, IEnumerable<CodeInstruction> codes, Func<CodeInstruction, bool> predicate) => codes.FindPrior(i, predicate);
    }
}
