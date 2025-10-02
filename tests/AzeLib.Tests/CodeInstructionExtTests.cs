using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using AzeLib.Extensions;
using HarmonyLib;
using Xunit;

namespace AzeLib.Tests
{
    public sealed class CodeInstructionExtTests
    {
        public static IEnumerable<object?[]> LocalStoreOpCodes()
        {
            yield return new object?[] { OpCodes.Stloc, OpCodes.Ldloc, CreateLocalBuilder() };
            yield return new object?[] { OpCodes.Stloc, OpCodes.Ldloc, (ushort)6 };
            yield return new object?[] { OpCodes.Stloc_S, OpCodes.Ldloc_S, CreateLocalBuilder() };
            yield return new object?[] { OpCodes.Stloc_S, OpCodes.Ldloc_S, (byte)3 };
            yield return new object?[] { OpCodes.Stloc_0, OpCodes.Ldloc_0, null };
            yield return new object?[] { OpCodes.Stloc_1, OpCodes.Ldloc_1, null };
            yield return new object?[] { OpCodes.Stloc_2, OpCodes.Ldloc_2, null };
            yield return new object?[] { OpCodes.Stloc_3, OpCodes.Ldloc_3, null };
        }

        [Theory]
        [MemberData(nameof(LocalStoreOpCodes))]
        public void GetLoadFromStore_ReturnsMatchingLoad(OpCode store, OpCode expectedLoad, object? operand)
        {
            var instruction = operand is null
                ? new CodeInstruction(store)
                : new CodeInstruction(store, operand);

            var result = instruction.GetLoadFromStore();

            Assert.Equal(expectedLoad, result.opcode);
            Assert.Equal(operand, result.operand);
        }

        [Fact]
        public void GetLoadFromStore_UnsupportedOpcode_Throws()
        {
            var instruction = new CodeInstruction(OpCodes.Starg, (short)0);

            Assert.Throws<InvalidOperationException>(() => instruction.GetLoadFromStore());
        }

        private static LocalBuilder CreateLocalBuilder()
        {
            var dynamicMethod = new DynamicMethod("LocalFactory", typeof(void), Type.EmptyTypes);
            return dynamicMethod.GetILGenerator().DeclareLocal(typeof(int));
        }
    }
}
