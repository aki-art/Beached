using Beached.Content;
using Beached.Content.BWorldGen;
using HarmonyLib;
using Klei;
using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace Beached.Patches.Worldgen
{
	public class SettingsCachePatch
	{
		[HarmonyPatch(typeof(SettingsCache), "LoadFiles", typeof(string), typeof(string), typeof(List<YamlIO.Error>))]
		public class SettingsCache_LoadFiles_Patch
		{
			[HarmonyPriority(Priority.High)]
			public static void Postfix()
			{
				Log.Info(" == Tweaking worldgen ==");

				if (SettingsCache.worlds?.worldCache == null) return;

				var list = new List<string>();

				foreach (var world in SettingsCache.worlds.worldCache)
				{
					if (world.Value.disableWorldTraits)
					{
						continue;
					}

					if (world.Key == "worlds/BeachedStart")
					{
						continue;
					}

					var traitRules = world.Value.worldTraitRules;

					world.Value.worldTraitRules ??= new List<ProcGen.World.TraitRule>();

					if (world.Value.worldTraitRules.Count == 0)
					{
						world.Value.worldTraitRules.Add(new ProcGen.World.TraitRule()
						{
							forbiddenTags = new() { BWorldGenTags.BeachedTraits.ToString() }
						});
					}
					else
					{
						foreach (var traitRule in traitRules)
						{
							traitRule.forbiddenTags ??= new List<string>();
							traitRule.forbiddenTags.Add(BWorldGenTags.BeachedTraits.ToString());
						}
					}

					list.Add(world.Key);
				}

				if (list.Count > 0)
				{
					Log.Info($"Added forbidden trait \"{BWorldGenTags.BeachedTraits}\" to these worlds from script.");

					foreach (var item in list)
					{
						Log.Info($"   - {item}");
					}
				}

#if ELEMENTS
				var metals = Elements.GetMetals().Select(e => e.ToString());

				Log.Info($"Adding Beached metals to Metal Poor & Metal Rich traits...");

				AddElementsToTrait("MetalPoor", metals);
				AddElementsToTrait("MetalRich", metals);
#endif
				Log.Info("== Worldgen Tweaking Done ==");
			}

			private static void AddElementsToTrait(string traitId, IEnumerable<string> elements)
			{
				if (SettingsCache.worldTraits.TryGetValue(traitId, out var trait))
				{
					if (trait.elementBandModifiers == null || trait.elementBandModifiers.Count == 0)
					{
						Log.Warning($"Could not add Beached metals to {traitId}.");
						return;
					}

					var massMultiplier = trait.elementBandModifiers[0].massMultiplier;
					var bandMultiplier = trait.elementBandModifiers[0].bandMultiplier;

					foreach (var metal in elements)
					{
						trait.elementBandModifiers.Add(new WorldTrait.ElementBandModifier()
						{
							element = metal,
							massMultiplier = massMultiplier,
							bandMultiplier = bandMultiplier
						});
					}
				}
			}
		}
	}
}
