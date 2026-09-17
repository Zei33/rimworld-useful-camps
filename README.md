# Useful Camps

A RimWorld 1.6 mod that gives camp maps the mining deposits an ordinary map
gets, so a camp is worth settling on for more than a night.

Unreleased. Not published to the Steam Workshop.

## How it works

Harmony patches in `1.6/Patches/CampResourcePatches.cs` add resource generation
to camp map creation. Harmony id `com.zei33.usefulcamps`.

## Building

Requires the `RimWorldDir` environment variable pointing at the RimWorld install,
the .NET SDK, and Mono for the 4.7.2 reference assemblies.

```sh
./build.sh
```

That builds Release and installs the mod into `$RimWorldDir/Mods/UsefulCamps`.
Build output under `1.6/Assemblies/net472/` is not tracked in git.

## Licence

GPL-3.0. See [LICENSE](LICENSE).
