using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace UsefulCamps.Patches
{
    /// <summary>
    /// Harmony patches to enable resource generation on camp maps.
    /// </summary>
    [HarmonyPatch]
    public static class CampResourcePatches
    {
        /// <summary>
        /// Patches GenStep_ScatterLumpsMineable.CalculateFinalCount to ensure camp maps generate resources
        /// like normal settlement maps rather than encounter maps by correcting the nomadic resource reduction.
        /// </summary>
        [HarmonyPatch(typeof(GenStep_ScatterLumpsMineable), "CalculateFinalCount")]
        [HarmonyPostfix]
        public static void CalculateFinalCount_Postfix(GenStep_ScatterLumpsMineable __instance, Map map, ref int __result)
        {
            // Check if this is a camp map and if the instance uses nomadic mineables and the map is not a starting map
            if (IsCampMap(map) && __instance.useNomadicMineables && !map.IsStartingMap)
            {
                // The original method reduced the resource count by the nomadic factor for non-starting maps
                // For camp maps, we want to undo this reduction by multiplying back up
                float nomadicFactor = Current.Game.storyteller.difficulty.nomadicMineableResourcesFactor;
                
                // Only apply correction if the factor was actually applied (less than 1.0)
                if (nomadicFactor < 1.0f && nomadicFactor > 0f)
                {
                    // Reverse the reduction by dividing by the factor (equivalent to multiplying by 1/factor)
                    __result = UnityEngine.Mathf.RoundToInt(__result / nomadicFactor);
                }
            }
        }

        /// <summary>
        /// Determines if the given map is a camp map.
        /// </summary>
        /// <param name="map">The map to check</param>
        /// <returns>True if the map is a camp map, false otherwise</returns>
        private static bool IsCampMap(Map map)
        {
            // Check if the map parent is a Camp
            return map?.Parent is Camp;
        }
    }
}