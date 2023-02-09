using Klei;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using ProcGenGame;
using UnityEngine;

namespace Beached.Patches.Worldgen
{
    internal class PureNoiseTerrain
    {
        public string biomeFilepath;
        public string noiseFilepath;
        private NoiseMap noiseMap;
        private BiomeSettings biomeSettings;
        private ElementBandConfiguration gradients;

        public PureNoiseTerrain(int seed)
        {
            var path = Mod.folder;
            biomeFilepath = System.IO.Path.Combine(path, "worldgen", "testBiome.yaml");
            noiseFilepath = System.IO.Path.Combine(path, "worldgen", "testNoise.yaml");

            LoadElementGradients();
            LoadNoise(seed, Grid.WidthInCells, Grid.HeightInCells);
            gradients = biomeSettings.TerrainBiomeLookupTable["Default"];
        }

        private void LoadNoise(int seed, int width, int height)
        {
            var tree = YamlIO.LoadFile<ProcGen.Noise.Tree>(noiseFilepath);
            //tree.BuildFinalModule(0);
            var plane = BuildNoiseSource(seed, width, height, tree);
            noiseMap = WorldGen.BuildNoiseMap(Vector3.zero, tree.settings.zoom, plane, width, height, null);
        }

        public NoiseMapBuilderPlane BuildNoiseSource(int seed, int width, int height, ProcGen.Noise.Tree tree)
        {
            var lowerBound = tree.settings.lowerBound;
            var upperBound = tree.settings.upperBound;

            var noiseMapBuilderPlane = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, false);
            noiseMapBuilderPlane.SetSize(width, height);
            noiseMapBuilderPlane.SourceModule = tree.BuildFinalModule(seed);

            return noiseMapBuilderPlane;
        }

        private TerrainCell.ElementOverride GetElementFromBiomeElementTable(int x, int y)
        {
            var num = noiseMap.GetValue(x, y);

            var elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), null);

            if (gradients.Count == 0)
            {
                return elementOverride;
            }

            for (var i = 0; i < gradients.Count; i++)
            {
                Debug.Assert(gradients[i].content != null, i.ToString());
                if (num < gradients[i].maxValue)
                {
                    return TerrainCell.GetElementOverride(gradients[i].content, gradients[i].overrides);
                }
            }

            return TerrainCell.GetElementOverride(gradients[gradients.Count - 1].content, gradients[gradients.Count - 1].overrides);
        }

        private void LoadElementGradients()
        {
            biomeSettings = YamlIO.LoadFile<BiomeSettings>(biomeFilepath);

            foreach (var config in biomeSettings.TerrainBiomeLookupTable)
            {
                config.Value.ConvertBandSizeToMaxSize();
            }
        }
    }
}