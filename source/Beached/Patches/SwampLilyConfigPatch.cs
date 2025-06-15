using Beached.Content;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class SwampLilyConfigPatch
	{
		[HarmonyPatch(typeof(SwampLilyConfig), "CreatePrefab")]
		public class SwampLilyConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				// the game adds it twice too
				AddPerplexiumToSafeElements(__result);
				__result.AddOrGet<KPrefabID>().prefabSpawnFn += AddPerplexiumToSafeElements;
			}

			private static void AddPerplexiumToSafeElements(GameObject go)
			{
				go.AddOrGet<PressureVulnerable>().safe_atmospheres.Add(ElementLoader.FindElementByHash(Elements.perplexium));
			}
		}
	}
}
