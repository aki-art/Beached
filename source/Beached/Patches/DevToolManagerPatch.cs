using HarmonyLib;

namespace Beached.Patches
{
	public class DevToolManagerPatch
	{
#if DEBUG
#if DEVTOOLS
		// forces the debug menu to be on by default
		[HarmonyPatch(typeof(DevToolManager), nameof(DevToolManager.UpdateShouldShowTools))]
		public class DevToolManager_UpdateShouldShowTools_Patch
		{
			public static void Postfix(ref bool ___showImGui) => ___showImGui = true;
		}
#endif
#endif
	}
}