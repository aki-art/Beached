using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using HarmonyLib;
using TUNING;

namespace Beached.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.LoadGeneratedEntities))]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				CROPS.CROP_TYPES.Add(new(JellyConfig.ID, 3f * CONSTS.CYCLE_LENGTH));
				CROPS.CROP_TYPES.Add(new(PipShootConfig.ID, 0.6f * CONSTS.CYCLE_LENGTH));
				CROPS.CROP_TYPES.Add(new(PalmLeafConfig.ID, 0.15f * CONSTS.CYCLE_LENGTH));
			}

			public static void Postfix()
			{
				MuffinConfig.OnPostEntitiesLoaded();
				BDb.OnPostEntitiesLoaded();
			}
		}
	}
}
