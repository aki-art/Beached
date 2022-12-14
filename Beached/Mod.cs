using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Defs.Entities.Plants;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TUNING;

namespace Beached
{
    public class Mod : UserMod2
    {
        public static bool DebugMode = true;

        public static Config Settings = new Config();

        public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            BTags.OnModLoad();

            ZoneTypes.Initialize();
            BeachedDevTools.Initialize();

            CROPS.CROP_TYPES.Add(new Crop.CropVal(CellAlgaeConfig.ID, 3f * CONSTS.CYCLE_LENGTH));
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach(var mod in mods)
            {
                if(mod.IsEnabledForActiveDlc())
                {
                    switch (mod.staticID)
                    {
                        case "TrueTiles":
                            Integration.TrueTiles.OnAllModsLoaded();
                            break;
                    }
                }
            }
        }
    }
}
