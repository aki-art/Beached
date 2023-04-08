using Beached.Content.BWorldGen;
using HarmonyLib;
using Klei;
using ProcGen;
using System.Collections.Generic;

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
                if (SettingsCache.worlds?.worldCache == null) return;

                foreach (var world in SettingsCache.worlds.worldCache)
                {
                    Log.Debug(world.Key);
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

                    if(world.Value.worldTraitRules.Count == 0)
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

                    foreach(var rule in world.Value.worldTraitRules)
                    {
                        Log.Debug(rule.forbiddenTags.Join());
                    }

                    Log.Debug("added forbidden trait \"BeachedCore\" to " + world.Key);
                }
            }
        }
    }
}
