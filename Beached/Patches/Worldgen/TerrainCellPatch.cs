using Beached.Content.BWorldGen;
using HarmonyLib;
using ProcGen;
using ProcGenGame;

namespace Beached.Patches.Worldgen
{
    internal class TerrainCellPatch
    {

        //[HarmonyPatch(typeof(TerrainCell), "ApplyBackground")]
        public class TerrainCell_ApplyBackground_Patch
        {
            public static bool Prefix(
                TerrainCell __instance,
                WorldGen worldGen,
                Chunk world,
                TerrainCell.SetValuesFunction SetValues,
                float temperatureMin,
                float temperatureRange,
                SeededRandom rnd)
            {
                var leaf = worldGen.GetLeafForTerrainCell(__instance);

                if (leaf.tags.Contains(BWorldGenTags.SmoothNoise))
                {
                    var node = __instance.node;
                    var availableTerrainPoints = __instance.availableTerrainPoints;
                    bool ignoreCaveOverride = leaf.tags.Contains(WorldGenTags.IgnoreCaveOverride);

                    var elementBandForBiome1 = worldGen.Settings.GetElementBandForBiome(node.type);
                    foreach (int availableTerrainPoint in availableTerrainPoints)
                    {
                        if(worldGen.HighPriorityClaimedCells.Contains(availableTerrainPoint))
                        {
                            continue;
                        }

                        if(ignoreCaveOverride && TrySetValuesWithCaveOverride(SetValues, availableTerrainPoint, world))
                        {
                            continue;
                        }

                        var xy = Grid.CellToXY(availableTerrainPoint);


                    }
                }

                return true;
            }

            private static bool TrySetValuesWithCaveOverride(TerrainCell.SetValuesFunction SetValues, int availableTerrainPoint, Chunk world)
            {
                var chunkOverride = world.overrides[availableTerrainPoint];

                if(chunkOverride < 100f)
                {
                    return false;
                }

                var element = chunkOverride switch
                {
                    >= 300 => WorldGen.voidElement,
                    >= 200 => WorldGen.unobtaniumElement,
                    _ => WorldGen.katairiteElement
                };

                SetValues(availableTerrainPoint, element, WorldGen.voidElement.defaultValues, Sim.DiseaseCell.Invalid);

                return true;
            }
        }
    }
}
