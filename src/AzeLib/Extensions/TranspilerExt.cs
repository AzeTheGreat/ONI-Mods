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
        /// <summary>
        /// Removes calls to <paramref name="toRemove"/> and cleans the evaluation stack while preserving control flow labels.
        /// </summary>
        /// <remarks>
        /// Any pending labels from removed calls with no replacement instructions are deferred to the next emitted instruction.
        /// </remarks>
        /// <param name="instructions">The original IL instructions.</param>
        /// <param name="toRemove">The method whose calls should be removed.</param>
        /// <returns>The modified instruction stream with balanced stack semantics.</returns>
        public static IEnumerable<CodeInstruction> MethodRemover(this IEnumerable<CodeInstruction> instructions, MethodInfo toRemove)
        {
            var pendingLabels = new List<Label>();

            foreach (var instruction in instructions)
            {
                if (pendingLabels.Count > 0)
                {
                    instruction.labels.InsertRange(0, pendingLabels);
                    pendingLabels.Clear();
                }

                if (!CallsTargetMethod(instruction, toRemove))
                {
                    yield return instruction;
                    continue;
                }

                var pops = BuildCleanupInstructions(toRemove);

                if (pops.Count > 0)
                {
                    instruction.MoveLabelsTo(pops[0]);
                    foreach (var pop in pops)
                        yield return pop;
                }
                else if (instruction.labels.Count > 0)
                {
                    pendingLabels.AddRange(instruction.labels);
                    instruction.labels.Clear();
                }
            }

            if (pendingLabels.Count > 0)
            {
                var nop = new CodeInstruction(OpCodes.Nop);
                nop.labels.AddRange(pendingLabels);
                pendingLabels.Clear();
                yield return nop;
            }
        }

        private static bool CallsTargetMethod(CodeInstruction instruction, MethodInfo toRemove)
        {
            if (!instruction.OperandIs(toRemove))
                return false;

            var opcode = instruction.opcode;
            return opcode == OpCodes.Call || opcode == OpCodes.Callvirt || opcode == OpCodes.Calli;
        }

        private static List<CodeInstruction> BuildCleanupInstructions(MethodInfo removedMethod)
        {
            var pops = new List<CodeInstruction>();

            if (!removedMethod.IsStatic)
                pops.Add(new CodeInstruction(OpCodes.Pop));

            var parameterCount = removedMethod.GetParameters().Length;
            for (var index = 0; index < parameterCount; index++)
                pops.Add(new CodeInstruction(OpCodes.Pop));

            return pops;
        }

        /// <summary>
        /// Applies <paramref name="function"/> to each instruction that satisfies <paramref name="predicate"/>.
        /// </summary>
        /// <remarks>
        /// Prefer combining this overload with <see cref="CodeInstructionExtensions.OperandIs(CodeInstruction, object)"/> when
        /// filtering by operand. A dedicated operand-targeting helper was removed after repeated reviews showed no call sites,
        /// so this overload intentionally remains the single entry point.
        /// </remarks>
        /// <param name="codes">The source instruction sequence.</param>
        /// <param name="predicate">Condition determining which instructions to replace.</param>
        /// <param name="function">Transformation for matched instructions.</param>
        /// <returns>The rewritten instruction sequence.</returns>
        public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> codes, Func<CodeInstruction, bool> predicate,
            Func<CodeInstruction, IEnumerable<CodeInstruction>> function)
        {
            foreach (var c in codes)
            {
                if (predicate(c))
                    foreach (var i in function(c))
                        yield return i;
                else
                    yield return c;
            }
        }

        public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> codes, Func<CodeInstruction, bool> predicate,
            Action<IEnumerable<CodeInstruction>, CodeInstruction> action) => codes.Manipulator(predicate, i => action(codes, i));
    }
}
