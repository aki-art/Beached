using Beached.Content;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class DesalinatorConfigPatch
	{
		[HarmonyPatch(typeof(DesalinatorConfig), "ConfigureBuildingTemplate")]
		public class DesalinatorConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				var murkyBrineToPollutedWater = go.AddComponent<ElementConverter>();

				murkyBrineToPollutedWater.consumedElements =
				[
					new ElementConverter.ConsumedElement(Elements.murkyBrine.CreateTag(), 5f)
				];

				murkyBrineToPollutedWater.outputElements =
				[
					new ElementConverter.OutputElement(3.5f, SimHashes.DirtyWater, 0f, storeOutput: true),
					new ElementConverter.OutputElement(1.5f, SimHashes.Salt, 0f, storeOutput: true)
				];
			}
		}
	}
}
