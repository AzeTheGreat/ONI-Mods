using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AzeLib.Extensions
{
    /// <summary>
    /// Helper extensions for working with Harmony <see cref="CodeInstruction"/> sequences.
    /// See <c>Extensions/README.md</c> for usage guidance and examples.
    /// </summary>
    public static class CodeInstructionExt
    {
        public static bool IsLocalOfType(this CodeInstruction i, Type type)
        {
            if (i.operand is LocalBuilder localBuilder && localBuilder.LocalType == type)
                return true;
            return false;
        }

        public static bool OpCodeIs(this CodeInstruction i, OpCode opCode) => i.opcode == opCode;

        private static readonly IReadOnlyDictionary<OpCode, OpCode> StoreToLoadMap = BuildStoreToLoadMap();

        private static IReadOnlyDictionary<OpCode, OpCode> BuildStoreToLoadMap()
        {
            var opCodes = typeof(OpCodes)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(OpCode))
                .Select(f => (OpCode)f.GetValue(null)!)
                .ToList();

            var opCodesByName = opCodes.ToDictionary(op => op.Name, op => op, StringComparer.Ordinal);
            var mapping = new Dictionary<OpCode, OpCode>();

            foreach (var kvp in opCodesByName)
            {
                if (!kvp.Key.StartsWith("stloc", StringComparison.Ordinal))
                    continue;

                var loadName = "ld" + kvp.Key.Substring(2);
                if (!opCodesByName.TryGetValue(loadName, out var loadOpcode))
                    continue;

                mapping[kvp.Value] = loadOpcode;
            }

            return mapping;
        }

        public static CodeInstruction GetLoadFromStore(this CodeInstruction i)
        {
            if (!StoreToLoadMap.TryGetValue(i.opcode, out var opCode))
                throw new InvalidOperationException($"Opcode '{i.opcode}' does not represent a supported local store.");

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
