using Beached.Content.Defs.Entities.Critters.Dreckos;
using Beached.Content.Defs.Entities.Critters.Karacoos;
using Beached.Content.Defs.Entities.Critters.Mites;
using Beached.Content.Defs.Entities.Critters.Pacus;
using Beached.Content.Defs.Entities.Critters.Rotmongers;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
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

				AddDietOptions(Assets.GetPrefab(PrehistoricPacuConfig.ID), PacuConfig.ID, [
					PrincessPacuConfig.ID,
					BabyPrincessPacuConfig.ID
				]);
			}
		}

		private static void AddDietOptions(GameObject prefab, string referencePrefabId, HashSet<Tag> preys)
		{
			var calorieMonitor = prefab.GetDef<CreatureCalorieMonitor.Def>();
			var diet = calorieMonitor.diet;
			var info = diet.infos.ToList().Find(info => info.consumedTags != null && info.consumedTags.Contains(referencePrefabId));
			if (info != null)
			{
				info.consumedTags = [.. info.consumedTags.Union(preys)];
			}

			prefab.GetDef<SolidConsumerMonitor.Def>().diet = diet;
			diet.UpdateSecondaryInfoArrays();
		}
	}
}
