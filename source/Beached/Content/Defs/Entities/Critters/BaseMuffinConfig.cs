using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	public class BaseMuffinConfig
	{
		public static GameObject CreatePrefab(string id, string name, string desc, string animFile, string symbolOverridePrefix = null)
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				name,
				desc,
				MuffinTuning.MASS,
				Assets.GetAnim(animFile),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				1,
				2,
				DECOR.BONUS.TIER2);

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				FactionManager.FactionID.Predator,
				id + "Original",
				CONSTS.NAV_GRID.WALKER_1X2,
				NavType.Floor,
				16,
				MuffinTuning.SPEED,
				MuffinTuning.ON_DEATH_DROP,
				2,
				true,
				true,
				GameUtil.GetTemperatureConvertedToKelvin(-60, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(50, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(-80, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(80, GameUtil.TemperatureUnit.Celsius));

			ConfigureAI(prefab, symbolOverridePrefix, BTags.Species.snail);

			prefab.AddTag(GameTags.Creatures.Walker);

			prefab.AddWeapon(2f, 3f);
			prefab.AddOrGetDef<CreatureFallMonitor.Def>();

			return prefab;
		}

		public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
		{
			var diet = new Diet(diet_infos.ToArray());
			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return prefab;
		}

		public static void ConfigureAI(GameObject gameObject, string symbolOverridePrefix, Tag species)
		{
			var choreTable = new ChoreTable.Builder()
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				//.Add(new GrowUpStates.Def())
				.Add(new TrappedStates.Def())
				//.Add(new IncubatingStates.Def())
				.Add(new BaggedStates.Def())
				//.Add(new FallStates.Def())
				//.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				.Add(new HunterStates.Def())
				.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new EatStates.Def())
				//.Add(new RanchedStates.Def())
				//.Add(new LayEggStates.Def())
				//.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(gameObject, choreTable, species, symbolOverridePrefix);
		}
	}
}
