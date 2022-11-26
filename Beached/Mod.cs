using Beached.Content;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
using System.IO;
using System.Reflection;

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

            Log.Debug("DEVTOOL MNGR INSTANCE");
            if (DevToolManager.Instance == null)
            {
                Log.Warning("it null");
            }
            var m_RegisterDevTool = AccessTools.Method(typeof(DevToolManager), "RegisterDevTool", new[] { typeof(DevTool), typeof(string) });
            if (m_RegisterDevTool != null)
            {
                m_RegisterDevTool.Invoke(DevToolManager.Instance, new object[] { new WorldGenDevTool(), "Mods/Beached/Worldgen" });
            }
            else
            {
                Log.Warning("NULL METHOD");
            }
        }
    }
}
