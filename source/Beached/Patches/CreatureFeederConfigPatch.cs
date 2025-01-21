using Beached.Content.Defs.Entities;
using HarmonyLib;

namespace Beached.Patches
{
	public class CreatureFeederConfigPatch
	{
		[HarmonyPatch(typeof(CreatureFeederConfig), "ConfigurePost")]
		public class CreatureFeederConfig_ConfigurePost_Patch
		{
			public static void Postfix(BuildingDef def) => BEntities.ConfigureCritterFeeder(def);
		}
	}
}
