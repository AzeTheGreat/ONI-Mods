# Compiling and Building

## Compiling

Compiling this solution is fairly simple.  If you run into issues, feel free to contact me for support.

- Clone the repo and install ONI.
- If ONI is not installed in the default Steam location (or your environment differs from the default):
  - In the `Solution Items` folder, add a new `.xml` file named `Directory.Build.props.user`.
  - Copy the contents of `.default` to `.user`.
  - Edit any properties specific to your environment in `.user`.
- Rebuild the solution.
  - Automatically pulls in required packages through nuget.
  - Creates publicized assemblies.
- If Visual Studio can't find the `_public.dll` assemblies:
  - Restart VS and reopen the solution.

## Building

Depending on the build configuration, the mods will be handled differently.

- Debug: 
  - Prefixed with `DEV_`
  - Exported to ONI's `mods\dev` folder.
- Release:
  - Exported to the Release folder
  - Zipped and placed in the [Distribute](https://github.com/AzeTheGreat/ONI-Mods/tree/master/Distribute) folder

## Library Documentation

- [AzeLib Harmony extension helpers](AzeLib/Extensions/README.md)
