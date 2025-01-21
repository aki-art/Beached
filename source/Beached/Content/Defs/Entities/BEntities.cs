using Beached.Content.Defs.Entities.Critters.Dreckos;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using TUNING;

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
			CROPS.CROP_TYPES.Add(new(PalmLeafConfig.ID, 0.05f * CONSTS.CYCLE_LENGTH, 6));
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
		}
	}
}
