namespace Beached.ModDevTools
{
    public class BeachedDevTools
    {
        // the game also tries to recognize DevTool classes by default, but it only adds them to the Debuggers menu, which is already overcrowsded
        // so this gets added by reflection separately
        /// also <see cref="Patches.DevToolMenuNodeListPatch.DevToolMenuNodeList_AddOrGetParentFor_Patch"/>
        public static void Initialize()
        {
            DevToolManager.Instance.RegisterDevTool<WorldGenDevTool>("Mods/Beached/Worldgen");
            DevToolManager.Instance.RegisterDevTool<DebugDevTool>("Mods/Beached/Debug");
            DevToolManager.Instance.RegisterDevTool<ConsoleDevTool>("Mods/Beached/Log");
        }
    }
}
