using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Xunit;

namespace AzeLib.Tests
{
    public sealed class TranspilerExtTests
    {
        [Fact]
        public void MethodRemover_InstanceCall_EmitsExpectedPops()
        {
            var target = typeof(RemovalTargets).GetMethod(nameof(RemovalTargets.InstanceWithArguments))!;
            var instructions = new List<CodeInstruction>
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Callvirt, target)
            };

            var result = instructions.MethodRemover(target).ToList();

            Assert.Equal(new[] { OpCodes.Ldarg_0, OpCodes.Ldarg_1, OpCodes.Ldarg_2, OpCodes.Pop, OpCodes.Pop, OpCodes.Pop }, result.Select(i => i.opcode));
        }

        [Fact]
        public void MethodRemover_StaticVoidCall_MovesLabelsToNextInstruction()
        {
            var target = typeof(RemovalTargets).GetMethod(nameof(RemovalTargets.StaticNoArguments))!;
            var label = new Label();
            var call = new CodeInstruction(OpCodes.Call, target);
            call.labels.Add(label);

            var instructions = new List<CodeInstruction>
            {
                call,
                new(OpCodes.Ret)
            };

            var result = instructions.MethodRemover(target).ToList();

            Assert.Single(result);
            Assert.Equal(OpCodes.Ret, result[0].opcode);
            Assert.Contains(label, result[0].labels);
        }

        [Fact]
        public void MethodRemover_StaticVoidCallAtEnd_PreservesLabelsWithNop()
        {
            var target = typeof(RemovalTargets).GetMethod(nameof(RemovalTargets.StaticNoArguments))!;
            var label = new Label();
            var call = new CodeInstruction(OpCodes.Call, target);
            call.labels.Add(label);

            var instructions = new List<CodeInstruction> { call };

            var result = instructions.MethodRemover(target).ToList();

            Assert.Single(result);
            Assert.Equal(OpCodes.Nop, result[0].opcode);
            Assert.Contains(label, result[0].labels);
        }

        private sealed class RemovalTargets
        {
            public void InstanceWithArguments(int first, int second)
            {
            }

            public static void StaticNoArguments()
            {
            }
        }
    }
}
