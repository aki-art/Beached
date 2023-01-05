using HarmonyLib;
using System.Reflection;

namespace Beached.ModDevTools
{
    public class BeachedDevTools
    {
        // the game also tries to recognize DevTool classes by default, but it only adds them to the Debuggers menu, which is already overcrowsded
        // so this gets added by reflection separately
        /// also <see cref="Patches.DevToolMenuNodeListPatch.DevToolMenuNodeList_AddOrGetParentFor_Patch"/>
        public static void Initialize()
        {
            var m_RegisterDevTool = AccessTools.DeclaredMethod(typeof(DevToolManager), "RegisterDevTool", new[]
            {
                typeof(string)
            });

            if (m_RegisterDevTool == null)
            {
                return;
            }

            RegisterTool<WorldGenDevTool>(m_RegisterDevTool, "Mods/Beached/Worldgen");
            RegisterTool<DebugDevTool>(m_RegisterDevTool, "Mods/Beached/Debug");
        }

        private static void RegisterTool<T>(MethodInfo registerMethod, string path) where T : DevTool, new()
        {
            var m_GenericRegisterDevTool = registerMethod.MakeGenericMethod(typeof(T));
            m_GenericRegisterDevTool.Invoke(DevToolManager.Instance, new object[] { path });
        }
    }
}
