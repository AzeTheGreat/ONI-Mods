using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AzeLib.Extensions
{
    public static class TranspilerExt
    {
        // TODO: Optimize this so it only pops if it needs to preserve a label?  Maybe even do label fixup?  Idk.
        // Pop off the parameters that would be consumed by the method and don't call the method.
        public static IEnumerable<CodeInstruction> MethodRemover(this IEnumerable<CodeInstruction> instructions, MethodInfo toRemove)
        {
            foreach (var i in instructions)
            {
                if (i.OperandIs(toRemove))
                {
                    // If is an instance method, pop the instance.
                    if(!toRemove.IsStatic)
                        yield return new CodeInstruction(OpCodes.Pop);
                    // Pop the parameters off the stack.
                    for (int p = 0; p < toRemove.GetParameters().Count(); p++)
                    {
                        yield return new CodeInstruction(OpCodes.Pop);
                    }
                }
                else
                    yield return i;
            }
        }

        public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> instructions, Func<CodeInstruction, bool> targeter,
            Func<CodeInstruction, IEnumerable<CodeInstruction>> manipulator)
        {
            foreach (var i in instructions)
            {
                if (targeter(i))
                    foreach (var result in manipulator(i))
                        yield return result;
                else
                    yield return i;
            }
        }

        public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> instructions, OpCode targetOpCode, object targetOperand,
            Func<CodeInstruction, IEnumerable<CodeInstruction>> manipulator) => instructions.Manipulator((CodeInstruction i) => i.Is(targetOpCode, targetOperand), manipulator);
        public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> instructions, object targetOperand,
            Func<CodeInstruction, IEnumerable<CodeInstruction>> manipulator) => instructions.Manipulator((CodeInstruction i) => i.OperandIs(targetOperand), manipulator);
        public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> instructions, OpCode targetOpCode,
            Func<CodeInstruction, IEnumerable<CodeInstruction>> manipulator) => instructions.Manipulator((CodeInstruction i) => i.OpCodeIs(targetOpCode), manipulator);
    }
}
