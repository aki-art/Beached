namespace Beached.ModDevTools
{
	public class BeachedDevTools
	{
		// the game also tries to recognize DevTool classes by default, but it only adds them to the Debuggers menu, which is already overcrowsded
		// so these get added manually
		/// also to fix nesting, <see cref="Patches.DevToolMenuNodeListPatch.DevToolMenuNodeList_AddOrGetParentFor_Patch"/>
		public static void Initialize()
		{
			DevToolManager.Instance.RegisterDevTool<WorldGenDevTool>("Mods/Beached/Worldgen");
			DevToolManager.Instance.RegisterDevTool<DebugDevTool>("Mods/Beached/Debug");
			DevToolManager.Instance.RegisterDevTool<ConsoleDevTool>("Mods/Beached/Log");
			DevToolManager.Instance.RegisterDevTool<NoisePreviewerDevTool>("Mods/Beached/Noise");
			DevToolManager.Instance.RegisterDevTool<BlueprintDevtool>("Mods/Beached/Blueprints");
			DevToolManager.Instance.RegisterDevTool<WFCDevTool>("Mods/Beached/WFC");
			DevToolManager.Instance.RegisterDevTool<LiquidShaderDevTool>("Mods/Beached/Liquid Shader");
		}
	}
}
