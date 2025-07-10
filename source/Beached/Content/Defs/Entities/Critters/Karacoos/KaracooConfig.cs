using Beached.Content.DefBuilders;
using Beached.Content.Defs.Foods;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.AI;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Karacoos
{
	public class KaracooConfig : BaseKaracooConfig, IEntityConfig
	{
		public const string ID = "Beached_Karacoo";
		public const string EGG_ID = "Beached_KaracooEgg";

		protected override string AnimFile => "beached_karacoo_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(this);

			prefab.AddOrGetDef<SittableEggsMonitor.Def>().maxSearchCost = 30;

			var diet = new Diet(BEntityTemplates.SimpleDiet(
				[Elements.moss.CreateTag(), Elements.fireMoss.CreateTag(), SimHashes.Algae.CreateTag()],
				SimHashes.Sucrose.CreateTag(),
				KaracooTuning.CALORIES_PER_KG_OF_ORE));

			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minConsumedCaloriesBeforePooping = KaracooTuning.CALORIES_PER_KG_OF_ORE * KaracooTuning.MIN_POOP_SIZE_IN_KG;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			prefab.GetComponent<Karacoo>().randomizeColors = true;

			return prefab;
		}

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Navigator(CritterBuilder.NAVIGATION.WALKER_1X2, 1f)
				.Egg(BabyKaracooConfig.ID, "beached_egg_karacoo_kanim")
					.Fertility(1f)
					.Incubation(5f)
					.Mass(1f)
					.EggChance(InfertileEggConfig.ID, 95f)
					.EggChance(EGG_ID, 5f)
					.Done();
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}

	}
}
