namespace Beached.Patches.DevToolsLinuxFix
{
	public class KImguiUtilPatch
	{
		/*		// https://github.com/asquared31415/ONITwitch/blob/main/ONITwitchCore/Patches/DevToolPatches.cs#L76C1-L85C3
				[HarmonyPatch(typeof(KImGuiUtil), nameof(KImGuiUtil.SetKAssertCB))]
				// ReSharper disable once InconsistentNaming
				public static class ImGui_Patch
				{
					private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
					{
						return [new CodeInstruction(OpCodes.Ret)];
					}
				}
		[HarmonyPatch(typeof(DevToolUI), nameof(DevToolUI.PingHoveredObject))]
		private static class DevToolNoPing
		{
			public static bool Prefix() => false;
		}
		*/
	}
}
