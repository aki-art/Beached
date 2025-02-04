using Database;
using HarmonyLib;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BGameplaySeasons
	{


		[HarmonyPatch(typeof(Klei.AI.MeteorShowerEvent.States), "CreateClusterMapMeteorShower")]
		public class MeteorShowerEvent_States_CreateClusterMapMeteorShower_Patch
		{
			public static void Prefix(MeteorShowerEvent.StatesInstance smi)
			{
				Log.Debug("starting meteor shower");
				Log.Debug(smi.gameplayEvent == null);
				Log.Debug(smi.gameplayEvent.clusterMapMeteorShowerID);


				GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
				Log.Debug(prefab == null);
			}
		}


		public const string ASTROPELAGOS = "Beached_AstropelagosMoonletMeteorShowers";
		public static GameplaySeason astropelagosMoonletMeteorShowers;

		public const string VANILLA_ASTROPELAGOS = "Beached_AstropelagoMeteorShowers";
		public static GameplaySeason vanillaAstropelagosMeteorShowers;

		[DbEntry]
		public static void Register(GameplaySeasons __instance)
		{
			VanillaSeasons(__instance);
			SpacedOutSeasons(__instance);
			//.AddEvent(BGameplayEvents.ClusterAbyssaliteShower);
		}

		private static void VanillaSeasons(GameplaySeasons __instance)
		{
			astropelagosMoonletMeteorShowers = __instance.Add(new MeteorShowerSeason(
				VANILLA_ASTROPELAGOS,
				GameplaySeason.Type.World,
				DlcManager.VANILLA_ID,
				20f,
				false,
				-1f,
				true)
				.AddEvent(BGameplayEvents.VanillaDiamondShower));
		}

		private static void SpacedOutSeasons(GameplaySeasons __instance)
		{
			astropelagosMoonletMeteorShowers = __instance.Add(new MeteorShowerSeason(
				ASTROPELAGOS,
				GameplaySeason.Type.World,
				DlcManager.EXPANSION1_ID,
				20f,
				false,
				-1f,
				true,
				-1,
				0f,
				float.PositiveInfinity,
				1,
				true,
				300f)
				.AddEvent(BGameplayEvents.ClusterDiamondShower));
		}
	}
}
