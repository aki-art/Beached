using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class CrabConfigPatch
	{
		[HarmonyPatch(typeof(CrabConfig), nameof(CrabConfig.CreatePrefab))]
		public class CrabConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				var crabLimpetHost = __result.AddOrGetDef<LimpetHost.Def>();
				crabLimpetHost.maxLevel = 3;
				crabLimpetHost.defaultGrowthRate = 0.025f;
				crabLimpetHost.itemDroppedOnShear = SimHashes.Lime.CreateTag();
				crabLimpetHost.massDropped = 30f;
				crabLimpetHost.diseaseIdx = Db.Get().Diseases.GetIndex(BDiseases.limpetEggs.id);
				crabLimpetHost.diseaseCount = 30000;
				crabLimpetHost.limpetKanim = "beached_pincher_limpetgrowth_kanim";
			}
		}
	}
}
