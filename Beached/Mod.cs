using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Defs.Entities.Plants;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
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
            RegisterDevTools();

            CROPS.CROP_TYPES.Add(new Crop.CropVal(CellAlgaeConfig.ID, 3f * CONSTS.CYCLE_LENGTH));
        }

        private static void RegisterDevTools()
        {
            var m_RegisterDevTool = AccessTools.DeclaredMethod(typeof(DevToolManager), "RegisterDevTool", new[]
            {
                typeof(string)
            },
            new[]
            {
                typeof(WorldGenDevTool)
            });

            if (m_RegisterDevTool != null)
            {
                m_RegisterDevTool.Invoke(DevToolManager.Instance, new object[] { "Mods/Beached/Worldgen" });
            }
        }
    }
}
