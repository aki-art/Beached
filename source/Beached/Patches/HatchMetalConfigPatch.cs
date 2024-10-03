using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class HatchMetalConfigPatch
	{
		[HarmonyPatch(typeof(HatchMetalConfig), "CreatePrefab")]
		public class HatchMetalConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				var tags = __result.AddComponent<AdditionalPoopTags>();
				tags.offset = new(0.25f, 0);
				tags.entries =
					[new()
						{
							tag = Elements.slag.CreateTag(),
							ratioToOutput = 1f/3f
						}];
				tags.displayMassMultiplier = 0.25f;
			}
		}
	}
}
