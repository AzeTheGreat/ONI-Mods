# Maintainer Playbook for Aze's Oxygen Not Included Mods

## Project Overview
- This repository hosts Aze's collection of Oxygen Not Included mods, ranging from UI tweaks like *Better Info Cards* to quality-of-life improvements such as *Default Building Settings* and *Suppress Notifications*. Review the root `README.md` for the complete catalog and distribution links before editing mods or assets.
- Detailed instructions for compiling and packaging the solution live in `src/README.md`. Treat that document as the canonical guide when preparing builds.

## Tooling & Build Workflow
> "Clone the repo and install ONI." (`src/README.md`)
>
> "If ONI is not installed in the default Steam location (or your environment differs from the default): In the `Solution Items` folder, add a new `.xml` file named `Directory.Build.props.user`. Copy the contents of `.default` to `.user`. Edit any properties specific to your environment in `.user`." (`src/README.md`)
>
> "Rebuild the solution. Automatically pulls in required packages through nuget. Creates publicized assemblies. If Visual Studio can't find the `_public.dll` assemblies: Restart VS and reopen the solution." (`src/README.md`)
>
> "Debug: Prefixed with `DEV_` ... exported to ONI's `mods\dev` folder. Release: Exported to the Release folder ... Zipped and placed in the Distribute folder." (`src/README.md`)

1. **Environment setup** — Follow the quoted steps above to align your build environment, including custom `Directory.Build.props.user` overrides when paths differ.
2. **Rebuild the solution** — Use Visual Studio/MSBuild to restore packages and emit publicized assemblies. Resolve missing `_public.dll` assemblies by restarting VS.
3. **Build outputs** — Expect DEV-prefixed debug builds in ONI's `mods\dev` directory and release zips under `Distribute/`.
4. **Diagnostics** — When instrumenting or benchmarking systems (e.g., `AzeLib` hooks), document findings in `NOTES.md`, and ensure any new DEBUG-only tooling is gated so release builds remain clean.

## Translation Workflow
> "Each translatable mod is a folder... You should see a `_template.pot` file, and any existing `.po` translations." (`Translations/README.md`)
>
> "You may translate through software intended for translation, or with just a text editor." (`Translations/README.md`)
>
> "Do not translate words inside curly braces... Do not translate words inside angle brackets." (`Translations/README.md`)
>
> "Save the file as `[language_code].po`." (`Translations/README.md`)
>
> "Please test your translations to make sure everything works as intended before submitting them... Restart the game so that it can reload translations." (`Translations/README.md`)

- Download `_template.pot` (and relevant `.po`) files for the target mod or use `Translations.zip` for bulk work.
- Translate with Poedit or a text editor while preserving placeholders and formatting markers.
- Finalize the `.po` file using the documented naming and optional attribution pattern.
- **Testing translations** — Inject the `.po` file into the mod's `Translations` folder within ONI's mods directory and restart the game to validate in-context before submitting.

## Tracking & Documentation Conventions
> "This document inventories the current in-code `TODO` comments across the repository. Items may be checked off only after the corresponding in-code `TODO` comment has been removed and the affected behavior has been re-validated through tests or manual verification. Keep this list synchronized whenever TODOs are introduced or resolved." (`ToDo.md`)

- **`NOTES.md`** — Log diagnostics, benchmarks, and investigation summaries with timestamps. Follow the existing structure that captures instrumentation context, measurement status, and next steps (see the dated sections already in the file).
- **`ToDo.md`** — Mirror the quoted policy: keep the tracker synchronized with in-code TODOs and validate behavior before checking items off.
- **Translations** — When adding or modifying `.po` files, note the work in `NOTES.md` if it involved testing or uncovered issues, and ensure translation guidelines above were followed.

## Testing Expectations
- **Mod code changes** — Rebuild the affected projects. When feasible, run automated tests under `tests/` or perform manual validation in-game. Document any blocked testing (e.g., missing runtime) in `NOTES.md` so maintainers can follow up (`NOTES.md` demonstrates how to record unexecuted diagnostics and next steps).
- **Localization changes** — Follow the translation testing workflow to confirm strings render correctly. If game-level validation is impossible, call that out explicitly in your PR and in `NOTES.md`.
- **Diagnostics/Instrumentation** — If adding diagnostics that cannot be executed in the current environment, record the required follow-up steps in `NOTES.md` and create or update entries in `ToDo.md` when action is needed.

## Coordination
- After completing work, commit changes and prepare a PR that references this playbook so other maintainers are aware. Use the documented workflows above to keep the knowledge base consistent.
