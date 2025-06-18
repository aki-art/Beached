using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using Klei.AI;
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
				crabLimpetHost.defaultGrowthRate = LimpetHost.GROWTH_RATE_6_CYCLES;
				crabLimpetHost.itemDroppedOnShear = SimHashes.Lime.CreateTag();
				crabLimpetHost.massDropped = 100f;
				crabLimpetHost.glandMass = 10f;
				crabLimpetHost.diseaseIdx = Db.Get().Diseases.GetIndex(BDiseases.limpetEggs.id);
				crabLimpetHost.diseaseCount = 30_000;
				crabLimpetHost.limpetKanim = "beached_pincher_limpetgrowth_kanim";
				crabLimpetHost.metabolismModifier = 1.2f;

				__result.GetComponent<Modifiers>().initialAmounts.Add(BAmounts.LimpetGrowth.Id);
			}
		}
	}
}
