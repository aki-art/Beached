using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class DigToolPatches
	{
		[HarmonyPatch(typeof(DigTool), "OnPrefabInit")]
		public class DigTool_OnPrefabInit_Patch
		{
			public static void Postfix(DigTool __instance)
			{
				Log.Debug("Beached_DigToolSkillToggle adding toggle here");
				__instance.gameObject.AddOrGet<Beached_DigToolSkillToggle>();
			}
		}

		[HarmonyPatch(typeof(DigTool), "OnActivateTool")]
		public class DigTool_OnActivateTool_Patch
		{
			public static void Postfix(DigTool __instance)
			{
				Log.Debug("activating dig tool");
				__instance.Trigger(ModHashes.onDigtoolActivated);
			}
		}

		[HarmonyPatch(typeof(DigTool), "OnDeactivateTool")]
		public class DigTool_OnDeactivateTool_Patch
		{
			public static void Postfix(DigTool __instance)
			{
				__instance.Trigger(ModHashes.onDigtoolDeActivated);
			}
		}

		[HarmonyPatch(typeof(DigTool), "PlaceDig")]
		public class DigTool_PlaceDig_Patch
		{
			public static void Postfix(DigTool __instance, int cell, ref GameObject __result)
			{
				if (__result == null)
				{
					Log.Debug("__result is null");
				}
				else
				{
					Log.Debug("__result EXISTS");
				}
				Log.Debug("on place dig");

				var instance = __instance;

				if (__instance == null)
				{
					instance = DigTool.Instance;
				}

				if (__instance == null || __result == null)
				{
					if (__result == null)
						Log.Debug("__result is null");
				}

				GameObject go;

				if (__result == null)
				{
					go = Grid.Objects[cell, (int)ObjectLayer.DigPlacer];
				}
				else
					go = __result;
				if (go == null)
				{
					Log.Debug("go is still null :(");
					return;
				}


				if (instance.TryGetComponent(out Beached_DigToolSkillToggle toggle)
					&& go.TryGetComponent(out Beached_DiggableSkillMod mod))
					mod.Configure(toggle.GetActiveFilter());
			}
		}
	}
}
