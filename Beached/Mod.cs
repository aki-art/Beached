global using Beached.Utils;
using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Defs.Entities.Plants;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
using Neutronium.PostProcessing.LUT;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TUNING;

namespace Beached
{

    public class Mod : UserMod2
    {
        public static bool debugMode = true;

        public static Config settings = new();

        public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

        public static bool isFastTrackHere;
        public static LUT_API lutAPI;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            BTags.OnModLoad();

            ZoneTypes.Initialize();
            BeachedDevTools.Initialize();
            BWorldGenTags.Initialize();
            
            lutAPI = LUT_API.Setup(harmony, true);
            
            CROPS.CROP_TYPES.Add(new(CellAlgaeConfig.ID, 3f * CONSTS.CYCLE_LENGTH));
        }


        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach (var modEntry in mods)
            {
                if (modEntry.IsEnabledForActiveDlc())
                {
                    switch (modEntry.staticID)
                    {
                        case "TrueTiles":
                            Integration.TrueTiles.OnAllModsLoaded();
                            break;
                        case "PeterHan.FastTrack":
                            isFastTrackHere = true;
                            break;
                    }
                }
            }
        }
    }
}
