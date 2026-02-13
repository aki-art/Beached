using Beached.Content.Defs.Entities.Critters.Dreckos;
using Beached.Content.Defs.Entities.Critters.Jellies;
using Beached.Content.Defs.Entities.Critters.Karacoos;
using Beached.Content.Defs.Entities.Critters.Mites;
using Beached.Content.Defs.Entities.Critters.Pacus;
using Beached.Content.Defs.Entities.Critters.Rotmongers;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.Scripts.Entities.AI;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class BEntities
	{
		public static void ConfigureCritterFeeder(BuildingDef def)
		{
			var crittersWithDiets = new Tag[]
			{
					BTags.Species.mite,
					BTags.Species.muffin,
					BTags.Species.snail
			};

			var storage = def.BuildingComplete.GetComponent<Storage>();
			storage.storageFilters.AddRange(DietManager.CollectDiets(crittersWithDiets).Keys);
		}

		public static void ConfigureCrops()
		{
			CROPS.CROP_TYPES.Add(new(JellyConfig.ID, 3f * CONSTS.CYCLE_LENGTH));
			CROPS.CROP_TYPES.Add(new(PipShootConfig.ID, 0.6f * CONSTS.CYCLE_LENGTH));
			CROPS.CROP_TYPES.Add(new(PalmLeafConfig.ID, 12f * CONSTS.CYCLE_LENGTH, 6));
			CROPS.CROP_TYPES.Add(new("Beached_PalmWood", 6f * CONSTS.CYCLE_LENGTH, 30));
			CROPS.CROP_TYPES.Add(new(RawKelpConfig.ID, 3f * CONSTS.CYCLE_LENGTH, 1));
		}

		public static void ModifyBaseEggChances()
		{
			DreckoTuning.EGG_CHANCES_BASE.Add(new FertilityMonitor.BreedingChance()
			{
				egg = MossyDreckoConfig.EGG_ID,
				weight = 0.1f
			});

			DreckoTuning.EGG_CHANCES_PLASTIC.Add(new FertilityMonitor.BreedingChance()
			{
				egg = MossyDreckoConfig.EGG_ID,
				weight = 0.1f
			});
		}

		public static void OnPostEntitiesLoaded()
		{
			MerpipConfig.ConfigureEggChancesToMerpip();

			if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID))
			{
				var raptor = Assets.GetPrefab(RaptorConfig.ID);

				raptor.AddTag(BTags.Creatures.muffinThreat);

				AddSolidDiet(raptor, new Diet.Info(
					[KibbleConfig.ID],
					SimHashes.BrineIce.CreateTag(),
					400_000f));


				AddDietOptions(raptor, HatchConfig.ID, [
					SlagmiteConfig.ID,
					BabySlagmiteConfig.ID,
					GleamiteConfig.ID,
					BabyGleamiteConfig.ID,
					KaracooConfig.ID,
					BabyKaracooConfig.ID,
					SlickShellConfig.ID,
					BabySlickShellConfig.ID,
					IronShellConfig.ID,
					BabyIronShellConfig.ID,
					MossyDreckoConfig.ID,
					BabyMossyDreckoConfig.ID,
					RotmongerConfig.ID,
					BabyRotmongerConfig.ID
				]);

				AddDietOptions(raptor, DinosaurMeatConfig.ID, [
					RawSnailConfig.ID,
					CracklingsConfig.ID,
					GelatineConfig.ID,
					HighQualityMeatConfig.ID
				]);

				var creatures = Assets.GetPrefabsWithComponent<CreatureBrain>();

				foreach (var creature in creatures)
				{
					var brain = creature.GetComponent<CreatureBrain>();
					if (brain.species == GameTags.Creatures.Species.SquirrelSpecies)
						AddDietOptions(creature, ForestTreeConfig.ID, [DewPalmConfig.ID]);
				}

				AddDietOptions(Assets.GetPrefab(PrehistoricPacuConfig.ID), PacuConfig.ID, [
					PrincessPacuConfig.ID,
					BabyPrincessPacuConfig.ID,
					MerpipConfig.ID,
					BabyMerpipConfig.ID,
					JellyfishConfig.ID,
					BabyJellyfishConfig.ID
				]);
			}
		}

		private static void AddSolidDiet(GameObject prefab, Diet.Info info)
		{
			var calorieMonitor = prefab.GetDef<CreatureCalorieMonitor.Def>();

			if (calorieMonitor == null)
				return;

			calorieMonitor.diet.infos = calorieMonitor.diet.infos.Append(info);

			calorieMonitor.diet.UpdateSecondaryInfoArrays();
		}

		/// <param name="addToAlreadyConsumedPrefabID">Add as an additional variant of an existing diet. Ie. Pips already eat Arbor Tree, so Dew Palm uses Arbot Tree here to add a second diet with the same configs.</param>
		private static void AddDietOptions(GameObject prefab, string addToAlreadyConsumedPrefabID, HashSet<Tag> consumedTags)
		{
			var calorieMonitor = prefab.GetDef<CreatureCalorieMonitor.Def>();

			if (calorieMonitor == null)
				return;

			var diet = calorieMonitor.diet;

			if (diet == null)
				return;

			var info = diet.infos.ToList().Find(info => info.consumedTags != null && info.consumedTags.Contains(addToAlreadyConsumedPrefabID));
			info?.consumedTags = [.. info.consumedTags.Union(consumedTags)];

			var solidConsumerMonitor = prefab.GetDef<SolidConsumerMonitor.Def>();
			if (solidConsumerMonitor != null)
				solidConsumerMonitor.diet = diet;

			var gasLiquidConsumerMonitor = prefab.GetDef<GasAndLiquidConsumerMonitor.Def>();
			if (gasLiquidConsumerMonitor != null)
				gasLiquidConsumerMonitor.diet = diet;

			var germConsumerMonitor = prefab.GetDef<GermConsumerMonitor.Def>();
			if (germConsumerMonitor != null)
				germConsumerMonitor.diet = diet;

			diet.UpdateSecondaryInfoArrays();
		}
	}
}
