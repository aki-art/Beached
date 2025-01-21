using Beached.Content;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class BaseMinionConfigPatch
	{

		[HarmonyPatch(typeof(BaseMinionConfig), "BaseMinion")]
		public class BaseMinionConfig_BaseMinion_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.AddTag(BTags.Creatures.doNotTargetMeByCarnivores);
			}
		}

		[HarmonyPatch(typeof(BaseMinionConfig), nameof(BaseMinionConfig.SetupLaserEffects))]
		public class MinionConfig_SetupLaserEffects_Patch
		{
			public static void Postfix(GameObject prefab)
			{
				ModAssets.Fx.Lasers.AddLaserEffects(prefab);
			}
		}
	}
}
