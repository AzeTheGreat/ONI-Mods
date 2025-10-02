# Outstanding TODO Tracker

This document inventories the current in-code `TODO` comments across the repository. Items may be checked off **only after** the corresponding in-code `TODO` comment has been removed **and** the affected behavior has been re-validated through tests or manual verification. Keep this list synchronized whenever TODOs are introduced or resolved.

## TODO Summary

| Status | Mod/Area | File | Summary |
| --- | --- | --- | --- |
| [x] | DefaultBuildingSettings | `src/DefaultBuildingSettings/OnBuild_Patch.cs` | Confirmed building defaults are stamped onto prefabs so no Harmony build hook is required. |
| [x] | AzeLib - Extensions | `src/AzeLib/Extensions/TranspilerExt.cs` | Optimize `MethodRemover` so it only emits stack pops when required and handles label preservation or fix-ups cleanly. |
| [x] | AzeLib - Extensions | `src/AzeLib/Extensions/TranspilerExt.cs` | Evaluate whether the operand-targeted `Manipulator` overload is sufficiently general to keep or should be removed. |
| [x] | AzeLib - Extensions | `src/AzeLib/Extensions/CodeInstructionExt.cs` | Provide documentation describing the available IL `CodeInstruction` extension methods. |
| [x] | AzeLib - Extensions | `src/AzeLib/Extensions/CodeInstructionExt.cs` | Refactor `GetLoadFromStore` into a cleaner, fully general solution for deriving load instructions from stores. |
| [x] | AzeLib - Attributes | `src/AzeLib/Attributes/AMonoBehaviour.cs` | Confirmed cached attribute-driven field wiring matches the base game's optimized approach. |
| [x] | AzeLib - Core | `src/AzeLib/AzeMod.cs` | Benchmark the `OnLoad` hook to ensure the reflection-based initialization does not unduly impact load times. |
| [x] | Build Tooling | `src/AutoIncrement.targets` | Modernized the AutoIncrement inline task to resolve `RoslynCodeTaskFactory` via `$(MSBuildToolsPath)` and hardened the JSON parser/serializer across runtimes. |
| [x] | BetterInfoCards - Util | `src/BetterInfoCards/Util/ResetPool.cs` | Consider adding logic to shrink the reset pool when demand drops. |
| [x] | BetterInfoCards - Converters | `src/BetterInfoCards/Converters/ConverterManager.cs` | Decide whether the default and title converters should live outside the shared converter dictionary for clarity or reuse. |

*Last updated: 2025-10-02 07:59 UTC*
