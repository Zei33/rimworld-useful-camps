using HarmonyLib;
using Verse;

namespace UsefulCamps
{
    /// <summary>
    /// Main mod entry point for the Useful Camps mod.
    /// Handles mod initialization and Harmony patching for camp resource generation.
    /// </summary>
    public class UsefulCampsMod : Mod
    {   
        /// <summary>
        /// The Harmony instance used for applying patches to the base game.
        /// </summary>
        private readonly Harmony harmony;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsefulCampsMod"/> class.
        /// Sets up Harmony patches and logs successful initialization.
        /// </summary>
        /// <param name="pack">The mod content pack containing mod information and assets.</param>
        public UsefulCampsMod(ModContentPack pack) : base(pack)
        {
            harmony = new Harmony("com.zei33.usefulcamps");
            harmony.PatchAll();

            Log.Message("[Useful Camps] Loaded version 1.0 successfully.");
        }
    }
}