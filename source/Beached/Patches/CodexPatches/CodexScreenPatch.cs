using Beached.Content.Codex;
using HarmonyLib;
using UnityEngine.UI;

namespace Beached.Patches.CodexPatches
{
	public class CodexScreenPatch
	{
		// configure and add custom codex widgets here
		[HarmonyPatch(typeof(CodexScreen), "SetupPrefabs")]
		public class CodexScreen_SetupPrefabs_Patch
		{
			public static void Postfix(CodexScreen __instance)
			{
				Beached_ArcheologyCodexWidget.ConfigurePrefab(__instance);

				FixElementTemperatureHeader(__instance);
			}

			// fixed the header of vanilla elements collapsing to 0
			private static void FixElementTemperatureHeader(CodexScreen instance)
			{
				var prefab = instance.PrefabTemperatureTransitionPanel;
				prefab.transform.Find("Bar").GetComponent<LayoutElement>().minHeight = 24.0f;
				//prefab.transform.Find("Bar").GetComponent<LocText>().autoSizeTextContainer = false;
			}
		}
	}
}
