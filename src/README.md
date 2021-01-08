# Compiling and Building

## Compiling

Compiling this solution is fairly simple.  Just duplicate 'Directory.Build.props.default`, rename it to `Directory.Build.props.user`, and edit any poperties that are different for you.

Since several references are imported through an MSBuild task, the solution may throw tons of errors claiming that references don't exist.  Just rebuild the solution once to import the references, and everything should compile.

## Building

When built in debug mode, mods will be prefixed with `DEV_` and exported to ONI's `mods\dev` folder.  
When built in release mode, mods will be exported to the Release folder, then will be zipped into release versions and placed in the [Distribute](https://github.com/AzeTheGreat/ONI-Mods/tree/master/Distribute) folder.