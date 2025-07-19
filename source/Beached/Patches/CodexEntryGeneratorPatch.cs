using Beached.Content.Defs;
using Beached.Content.ModDb.Germs;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class CodexEntryGeneratorPatch
	{
		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateDiseaseDescriptionContainers")]
		public class CodexEntryGenerator_GenerateDiseaseDescriptionContainers_Patch
		{
			public static void Postfix(Klei.AI.Disease disease, List<ContentContainer> containers)
			{
				if (disease == null)
					return;

				if (disease.IdHash == BDiseases.limpetEggs.IdHash || disease.IdHash == BDiseases.plankton.IdHash)
					CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(disease.Id, containers);
			}
		}

		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateGeyserEntries")]
		public class CodexEntryGenerator_GenerateGeyserEntries_Patch
		{
			public static void Postfix(Dictionary<string, CodexEntry> __result)
			{
				if (__result.TryGetValue(GeyserConfigs.PrefabID(GeyserConfigs.SALT_VOLCANO), out var entry))
					CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(GeyserConfigs.SALT_VOLCANO, entry.contentContainers);
			}
		}
	}
}
