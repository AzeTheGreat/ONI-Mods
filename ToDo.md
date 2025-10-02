# Outstanding TODO Tracker

This document inventories the current in-code `TODO` comments across the repository. Items may be checked off **only after** the corresponding in-code `TODO` comment has been removed **and** the affected behavior has been re-validated through tests or manual verification. Keep this list synchronized whenever TODOs are introduced or resolved.

## TODO Summary

| Status | Mod/Area | File | Summary |
| --- | --- | --- | --- |
| [ ] | DefaultBuildingSettings | `src/DefaultBuildingSettings/OnBuild_Patch.cs` | Revisit whether building configs can now be edited directly instead of relying on the current Harmony patch approach. |
| [x] | AzeLib - Extensions | `src/AzeLib/Extensions/TranspilerExt.cs` | Optimize `MethodRemover` so it only emits stack pops when required and handles label preservation or fix-ups cleanly. |
| [ ] | AzeLib - Extensions | `src/AzeLib/Extensions/TranspilerExt.cs` | Evaluate whether the operand-targeted `Manipulator` overload is sufficiently general to keep or should be removed. |
| [ ] | AzeLib - Extensions | `src/AzeLib/Extensions/CodeInstructionExt.cs` | Provide documentation describing the available IL `CodeInstruction` extension methods. |
| [ ] | AzeLib - Extensions | `src/AzeLib/Extensions/CodeInstructionExt.cs` | Refactor `GetLoadFromStore` into a cleaner, fully general solution for deriving load instructions from stores. |
| [ ] | AzeLib - Attributes | `src/AzeLib/Attributes/AMonoBehaviour.cs` | Improve the attribute-driven field wiring to match the base game's optimized approach. |
| [ ] | AzeLib - Core | `src/AzeLib/AzeMod.cs` | Benchmark the `OnLoad` hook to ensure the reflection-based initialization does not unduly impact load times. |
| [ ] | Build Tooling | `src/AutoIncrement.targets` | Diagnose intermittent `RoslynCodeTaskFactory` reference resolution failures and modernize the task implementation with newer C# features. |
| [ ] | BetterInfoCards - Util | `src/BetterInfoCards/Util/ResetPool.cs` | Consider adding logic to shrink the reset pool when demand drops. |
| [ ] | BetterInfoCards - Converters | `src/BetterInfoCards/Converters/ConverterManager.cs` | Decide whether the default and title converters should live outside the shared converter dictionary for clarity or reuse. |

*Last updated: 2025-10-02 04:23 UTC*
