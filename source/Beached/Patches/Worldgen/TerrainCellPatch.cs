using Beached.Content;
using Beached.Content.BWorldGen;
using HarmonyLib;
using ProcGen;
using ProcGenGame;

namespace Beached.Patches.Worldgen
{
	internal class TerrainCellPatch
	{
		[HarmonyPatch(typeof(TerrainCell), "ApplyBackground")]
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

				if (leaf.tags.Contains(BWorldGenTags.WaveFunctionCollapse))
				{
					var rubber = Elements.rubber.Get();
					var node = __instance.node;

					var availableTerrainPoints = __instance.availableTerrainPoints;

					foreach (var availableTerrainPoint in availableTerrainPoints)
					{
						if (worldGen.HighPriorityClaimedCells.Contains(availableTerrainPoint))
						{
							continue;
						}

						SetValues(availableTerrainPoint, rubber, rubber.defaultValues, Sim.DiseaseCell.Invalid);
					}

					//HandleSurfaceModifier(worldGen.Settings, __instance, BWorldGenTags.SandBeds, world, SetValues, rnd);

					return false;
				}

				return true;
			}

			private static void HandleSurfaceModifier(
				WorldGenSettings settings,
				TerrainCell terrainCell,
				Tag targetTag,
				Chunk world,
				TerrainCell.SetValuesFunction SetValues,
				SeededRandom rnd)
			{
				/*                var element = ElementLoader.FindElementByName(settings.GetFeature(targetTag.Name)
									.GetOneWeightedSimHash("SurfaceChoices", rnd)
									.element);*/

				var element = ElementLoader.FindElementByHash(SimHashes.Sand);
				var defaultValues = element.defaultValues;
				var invalid = Sim.DiseaseCell.Invalid;

				foreach (var cell in terrainCell.availableTerrainPoints)
				{
					//Log.Debug(Grid.Element[cell].id);
				}
			}
		}
	}
}
