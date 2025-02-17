﻿using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	public class ClusterMapMeteorShowerConfigPatch
	{
		[HarmonyPatch(typeof(ClusterMapMeteorShowerConfig), nameof(ClusterMapMeteorShowerConfig.CreatePrefabs))]
		public class ClusterMapMeteorShowerConfig_CreatePrefabs_Patch
		{
			public static void Postfix(List<GameObject> __result)
			{
				if (DlcManager.IsExpansion1Active())
				{
					__result.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor(
						"Beached_Diamond",
						BGameplayEvents.DIAMOND_SHOWER,
						STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BEACHED_DIAMOND.NAME,
						STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BEACHED_DIAMOND.DESCRIPTION,
						"beached_shower_diamond_kanim",
						"idle_loop",
						"ui",
						DlcManager.AVAILABLE_EXPANSION1_ONLY,
						SimHashes.Unobtanium));

					__result.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor(
						"Beached_Abyssalite",
						BGameplayEvents.ABYSSALITE_METEOREVENTID,
						STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BEACHED_ABYSSALITE.NAME,
						STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BEACHED_ABYSSALITE.DESCRIPTION,
						"shower_cluster_biological_kanim",
						"idle_loop",
						"ui",
						DlcManager.AVAILABLE_EXPANSION1_ONLY,
						SimHashes.Unobtanium));
				}
			}
		}
	}
}
