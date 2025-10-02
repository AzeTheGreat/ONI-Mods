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
