using Beached.Content.Codex;
using Beached.Content.Defs;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts;
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


		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateRoleEntries")]
		public class CodexEntryGenerator_GenerateRoleEntries_Patch
		{
			public static void Postfix(ref Dictionary<string, CodexEntry> __result)
			{
				if (__result.TryGetValue(BSkills.ARCHEOLOGY_ID, out var archeology))
				{
					var widgets = new List<ICodexWidget>();
					foreach (var chance in Beached_Mod.Instance.treasury.chances)
					{
						Log.Debug($"adding element: {chance.Key}");
						var element = ElementLoader.FindElementByHash(chance.Key);
						if (element == null)
						{
							Log.Debug($"element not found: {chance.Key}");
							continue;
						}

						widgets.Add(Beached_ArcheologyCodexWidget.Generate(element.tag, chance.Value));
					}

					var contents = new ContentContainer(widgets, ContentContainer.ContentLayout.Vertical);

					archeology.contentContainers.Add(new ContentContainer(
					[
						new CodexSpacer(),
						new CodexCollapsibleHeader("Material Yields", contents) // TODO strings
					], ContentContainer.ContentLayout.Vertical));

					archeology.contentContainers.Add(contents);
				}
				else Log.Debug("no archeology entry");
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
