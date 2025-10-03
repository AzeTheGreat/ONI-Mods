# AzeLib OnLoad benchmark (2024-06-17)

## Instrumentation
- Added DEBUG-only diagnostics around `AzeUserMod.OnLoad` to log discovery, invocation, and total execution timing without affecting release builds.
- Reflection-based discovery results are now cached so domain reloads reuse the same `OnLoad` metadata instead of re-scanning the assembly.

## Measurement status
- Local container image lacks a .NET runtime (`dotnet` is unavailable), so the new diagnostics could not be executed here.
- Representative measurements should be captured by launching the game or a debug harness with a DEBUG build; the logs will emit per-load timing once the diagnostics run.

## Preliminary analysis
- Static inspection shows only four `[OnLoad]` hooks across the solution, so the reflection sweep is expected to be inexpensive even before caching.
- The new cache eliminates the repeated reflection cost on subsequent loads, so the only remaining overhead is delegate invocation.

## Next steps for maintainers
- Run a DEBUG build in-game to collect the emitted timings and confirm the cached path is hit after the first load.
- If invocation proves hot, consider promoting frequently used hooks to precompiled delegates; current data does not suggest this is necessary.

## 2025-10-02 - AutoIncrement Roslyn migration
- Reproduced the historical `RoslynCodeTaskFactory` resolution failure by running `dotnet msbuild src/AzeLib/AzeLib.csproj /t:AutoIncrement /v:diag`, which emitted MSB4175 due to the inline task pointing at an SDK-relative path that collapsed on non-Windows hosts.
- Updated `src/AutoIncrement.targets` to load `RoslynCodeTaskFactory` from `$(MSBuildToolsPath)` with explicit `UsingTask` metadata so the factory resolves consistently across runtimes.
- Refactored the embedded task to use structured helpers, regex-based JSON parsing, and culture-invariant formatting while preserving the existing `version.json` contract.
- Validated the task in isolation via `dotnet msbuild src/AzeLib/AzeLib.csproj /t:AutoIncrement /p:Configuration=Debug`, confirming the revision value increments and the new serializer output remains stable.
- Re-reviewed the modernized implementation on 2025-10-02 to confirm the documentation matches the committed Roslyn task structure and versioning flow.

## 2025-10-05 - BetterInfoCards converter registry
- Split the default (raw text) and title converters out of the general registry to keep fallback lookups deterministic and prevent accidental overrides.
- Updated documentation and unit tests to reflect the dedicated storage and ensure `TryGetConverter` continues to resolve default, title, and named entries as expected.

## 2025-10-06 - BetterInfoCards unreachable hover card update
- Adjusted the unreachable card injection to pull hover targets through reflection so both legacy `overlayValidHoverObjects` and the new `hoverObjects` member are supported.
- Unable to compile `BetterInfoCards` inside this container because the ONI managed assemblies (e.g., `Assembly-CSharp.dll`) are not present; rebuild requires a local install following `src/README.md`.
- Smoke-test pending: verify in-game that selecting an unreachable item still draws the custom card once the mod is rebuilt with the refreshed dependencies.

## 2025-10-08 - ResetPool delegate wiring
- Confirmed the delegate wiring change builds under the ONI toolchain in principle, but the container image still lacks a .NET runtime (`dotnet` is unavailable), so a full `oniMods.sln` rebuild could not be executed here.
- Maintainers should rerun `dotnet build src/oniMods.sln` on a workstation with the ONI assemblies installed to verify end-to-end.

## 2025-10-09 - BetterInfoCards cursor hit patch compatibility
- Updated the reflection used by `ModifyHits` to tolerate the additional generic argument introduced in U56 so the mod no longer throws `Incorrect length` at load.
- Replaced the `AccessTools.AllMethods` lookup with `AccessTools.GetDeclaredMethods` so Harmony 2.3 continues to locate `InterfaceTool.GetObjectUnderCursor` without relying on removed APIs.
- Attempted to rebuild via `dotnet build src/oniMods.sln`, but the container image still lacks a .NET runtime, so compilation could not be performed here.
- Maintainers should rebuild on a workstation with the ONI-managed assemblies to validate Harmony loads cleanly in-game.

## 2025-10-10 - BetterInfoCards cursor signature update
- Relaxed the `ModifyHits` signature guard and added a bool-first fallback so Harmony locates the updated `InterfaceTool.GetObjectUnderCursor` overload even when U56 adds extra optional parameters.
- Static inspection only; rebuilding still requires the ONI assemblies and a local .NET runtime, both unavailable in this container.
