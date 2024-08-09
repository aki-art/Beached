using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.OnPrefabInit))]
		public class SaveLoader_OnPrefabInit_Patch
		{
			public static void Postfix(SaveLoader __instance)
			{
				__instance.gameObject.AddOrGet<Beached_WorldLoader>();
			}
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), typeof(IReader))]
		public class SaveLoader_Load_Patch
		{
			public static void Postfix(SaveLoader __instance)
			{
				Beached_WorldLoader.Instance.WorldLoaded(__instance.GameInfo.clusterId);
			}
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.LoadFromWorldGen))]
		public class SaveLoader_LoadFromWorldGen_Patch
		{
			public static void Postfix(SaveLoader __instance)
			{
				Beached_WorldLoader.Instance.WorldLoaded(__instance.Cluster.Id);
			}
		}
	}
}
