# AzeLib Extension Utilities

## `CodeInstructionExt`

The `CodeInstructionExt` helpers streamline Harmony transpiler authoring by wrapping common `CodeInstruction`
patterns. The extension methods available are:

- [`IsLocalOfType`](#islocaloftype)
- [`OpCodeIs`](#opcodeis)
- [`GetLoadFromStore`](#getloadfromstore)
- [`MakeNop`](#makenop)
- [`FindNext`](#findnext)
- [`FindPrior`](#findprior)

Each section below summarizes the intent, parameters, and provides a modding-oriented usage example.

### `IsLocalOfType`

Determines whether an instruction references a local variable of a specific type.

- **Parameters:** `Type type` – the expected local variable type.
- **Returns:** `true` when the operand is a `LocalBuilder` with a matching `LocalType`.

**Example – Skip caching locals that already contain the expected type:**

```csharp
public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
{
    var cachedState = generator.DeclareLocal(typeof(OperationalState));

    foreach (var code in instructions)
    {
        if (code.IsLocalOfType(typeof(OperationalState)))
        {
            // Replace with our cached local reference
            yield return new CodeInstruction(OpCodes.Ldloc_S, cachedState);
            continue;
        }

        yield return code;
    }
}
```

### `OpCodeIs`

Compares the instruction opcode against a specific `OpCode`.

- **Parameters:** `OpCode opCode` – the opcode to test for equality.
- **Returns:** `true` when `i.opcode == opCode`.

**Example – Guard against multiple patch applications:**

```csharp
if (instruction.OpCodeIs(OpCodes.Call) && instruction.operand is MethodInfo target && target.Name == "Update")
{
    // Inject our marker only once.
}
```

### `GetLoadFromStore`

Produces the matching load instruction for a local store opcode, preserving the operand.

- Automatically supports every `stloc*` variant (numbered, short form, and address forms) by deriving the corresponding load opcode from IL metadata.
- Throws an `InvalidOperationException` when invoked with an opcode that does not represent a supported local store.
- **Returns:** A new `CodeInstruction` that loads the same local variable just stored.

**Example – Re-read a cached local immediately after writing it:**

```csharp
foreach (var instruction in instructions)
{
    if (instruction.OpCodeIs(OpCodes.Stloc_S) && instruction.operand is LocalBuilder)
    {
        yield return instruction;
        yield return instruction.GetLoadFromStore();
        continue;
    }

    yield return instruction;
}
```

### `MakeNop`

Mutates an instruction in-place to become `OpCodes.Nop`.

- **Returns:** The same `CodeInstruction` instance for fluent modification.

**Example – Remove an unwanted call while retaining list size:**

```csharp
foreach (var instruction in instructions)
{
    if (instruction.OpCodeIs(OpCodes.Call) && instruction.operand is MethodInfo oldCall && oldCall.Name == "ConsumeEnergy")
    {
        yield return instruction.MakeNop();
        continue;
    }

    yield return instruction;
}
```

### `FindNext`

Locates the next instruction that satisfies the supplied predicate, starting from the current instruction.

- **Parameters:**
  - `IEnumerable<CodeInstruction> codes` – the instruction list being manipulated.
  - `Func<CodeInstruction, bool> predicate` – condition used to find the next matching instruction.
- **Returns:** The next matching `CodeInstruction`, or `null` if none is found.

**Example – Insert a condition before the next branch:**

```csharp
var instructionsList = instructions.ToList();

foreach (var instruction in instructionsList)
{
    var nextBranch = instruction.FindNext(instructionsList, ci => ci.opcode.FlowControl == FlowControl.Cond_Branch);
    if (nextBranch != null)
    {
        // Inject our guard before the branch
    }
}
```

### `FindPrior`

Locates the prior instruction that satisfies the supplied predicate, moving backwards from the current instruction.

- **Parameters:**
  - `IEnumerable<CodeInstruction> codes` – the instruction list being manipulated.
  - `Func<CodeInstruction, bool> predicate` – condition used to find the previous matching instruction.
- **Returns:** The previous matching `CodeInstruction`, or `null` if none is found.

**Example – Anchor injection relative to the previous load:**

```csharp
var instructionsList = instructions.ToList();

foreach (var instruction in instructionsList)
{
    var previousLoad = instruction.FindPrior(instructionsList, ci => ci.opcode == OpCodes.Ldarg_0);
    if (previousLoad != null)
    {
        // Add our sanity check just after the last argument load
    }
}
```
