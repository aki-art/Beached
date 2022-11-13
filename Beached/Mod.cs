using HarmonyLib;
using KMod;
using System.IO;
using System.Reflection;

namespace Beached
{
    public class Mod : UserMod2
    {
        public static bool DebugMode = true;

        public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }
    }
}
