using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.LoadGeneratedEntities))]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Postfix()
			{
				MuffinConfig.OnPostEntitiesLoaded();
				BDb.OnPostEntitiesLoaded();
			}
		}
	}
}
