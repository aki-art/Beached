using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities;
using HarmonyLib;

namespace Beached.Patches
{
	public class SaveGamePatch
	{
		[HarmonyPatch(typeof(SaveGame), nameof(SaveGame.OnPrefabInit))]
		public class SaveGame_OnPrefabInit_Patch
		{
			public static void Postfix(SaveGame __instance)
			{
				__instance.gameObject.AddOrGet<Beached_Mod>();
				__instance.gameObject.AddOrGet<Beached_Grid>();
				__instance.gameObject.AddOrGet<DepthsVeil>();
				__instance.gameObject.AddOrGet<Beached_ElectricityRenderer>();
			}
		}
	}
}
