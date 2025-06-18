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
				if (disease != null && disease.IdHash == BDiseases.limpetEggs.IdHash)
				{
					CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(disease.Id, containers);
				}
			}
		}
	}
}
