using Beached.Content.ModDb;
using HarmonyLib;
using Klei.AI;

namespace Beached.Content.Scripts.Entities
{
	public class CropPatch
	{
		[HarmonyPatch(typeof(Crop), "OnSpawn")]
		public class Crop_OnSpawn_Patch
		{
			public static void Postfix(Crop __instance)
			{
				var myWorld = __instance.GetMyWorld();
				if (!__instance.TryGetComponent(out Effects effects))
					return;

				if (myWorld.WorldTraitIds.Contains("traits/Beached_Damp"))
				{
					if (effects.HasEffect(BEffects.DAMP_PLANTGROWTH))
						return;

					effects.Add(BEffects.DAMP_PLANTGROWTH, true);
				}
				else if (myWorld.WorldTraitIds.Contains("traits/Beached_Arid"))
				{
					if (effects.HasEffect(BEffects.ARID_PLANTGROWTH))
						return;

					effects.Add(BEffects.ARID_PLANTGROWTH, true);
				}
			}
		}
	}
}
