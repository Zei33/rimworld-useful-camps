# UsefulCamps

Gives camp maps the mineable lumps an ordinary map gets, instead of the reduced count RimWorld hands to
nomadic maps. 77 lines, one Harmony patch. Unreleased. It had no git repository at all before 2026-09-17:
the source was recovered from `$RimWorldDir/Mods/UsefulCamps` and committed as a single initial commit.
Never reviewed or play-tested since. No DLC required.

Harmony id `com.zei33.usefulcamps`, namespace `UsefulCamps`. No defs, no textures, no user-facing
strings; there are nine `UsefulCamps_Keys.xml` files, the English one carrying a comment saying so and
the other eight an empty `<LanguageData>`.

## Types and patches

| File | Type | Target |
| --- | --- | --- |
| `1.6/ModEntry.cs` | `UsefulCampsMod : Mod` | Constructs Harmony, `PatchAll()` |
| `1.6/Patches/CampResourcePatches.cs` | Harmony postfix | `GenStep_ScatterLumpsMineable.CalculateFinalCount(Map)` |

The postfix fires when `map.Parent is Camp`, the genstep has `useNomadicMineables`, and the map is not the
starting map. It then divides `__result` by `Current.Game.storyteller.difficulty.nomadicMineableResourcesFactor`,
undoing the reduction vanilla applied.

## What vanilla actually does

`GenStep_ScatterLumpsMineable.CalculateFinalCount` has two branches:

```
if (count >= 0)            return Round(count * GetPlacementFactor(map));
num = countPer10kCellsRange.RandomInRange;
if (!map.IsStartingMap && useNomadicMineables) num *= difficulty.nomadicMineableResourcesFactor;
return Round(CountFromPer10kCells(num, map) * GetPlacementFactor(map));
```

The nomadic factor is only applied on the second branch, the one where `count` is left at its default of
-1. The postfix does not check which branch ran, so a genstep configured with an explicit `count` and
`useNomadicMineables` would have its result inflated although nothing was ever reduced. No vanilla def in
`$RimWorldDir/Data` sets `useNomadicMineables` in XML, so whether that combination occurs in play is
unverified. Worth reading before touching this patch.

`nomadicMineableResourcesFactor` is 1 on the easiest difficulty, 0.75 on the next, 0.5 on the rest
(`Data/Core/Defs/Misc/DifficultyDefs/Difficulties.xml`). At 1 the guard `nomadicFactor < 1.0f` skips the
correction, which is correct.

`RimWorld.Planet.Camp` is base game, defined in `Data/Core/Defs/WorldObjectDefs/WorldObjects.xml`, so the
mod needs no DLC. `GenStep_PreciousLump` inherits `GenStep_ScatterLumpsMineable` and does not override
`CalculateFinalCount`, so the patch applies to it too.

## Build

```sh
export FrameworkPathOverride=/opt/homebrew/opt/mono/lib/mono/4.7.2-api
dotnet build rimworld-useful-camps.sln -c Release
```

`./build.sh` builds Release then deletes and replaces `$RimWorldDir/Mods/UsefulCamps`, where the recovered
copy still lives. Destructive to that folder.

`UsefulCamps.csproj` is the canonical project file. Since 2026-09-17 all eight repos carry the same one,
`rimworld-mod-template/ModTemplate.csproj` included, differing only in the project name. The build is
clean: zero warnings, and `1.6/Assemblies/net472` ends up holding only the mod's own `.dll` and `.pdb`.
`rimworld-mod-template/CLAUDE.md` explains what the project file and `build.sh` guarantee and why, which
is worth reading before changing either here.

## Unfinished

- Never confirmed in game that a camp map actually generates more lumps.
- `Documentation/Architecture.md` and `Features.md` are still the scaffold's, headed "Mod Template".
- Nine empty `ModTemplate_Keys.xml` files sit alongside the `UsefulCamps_Keys.xml` ones.
- No Workshop id, no preview work.
