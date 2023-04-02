using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using VoronoiTree;

namespace Beached.ModDevTools
{
    internal class WorldLayoutHelper
    {
        public WorldGen Generate(int seed)
        {
            SettingsCache.LoadFiles(new List<Klei.YamlIO.Error>());
            var worldGen = new WorldGen("worlds/BeachedTinyDevTest", new List<string>(), new List<string>(), false);

            //worldGen.GenerateLayout((_, _, _) => true);
            //worldGen.FinalizeStartLocation();

            Log.Debug("world X: " + worldGen.data?.world?.size.X);
            var layout = new WorldLayout(worldGen, 224, 224, seed);
            worldGen.data.worldLayout = layout;

            WorldLayout.SetLayerGradient(SettingsCache.layers.LevelLayers);
            layout.voronoiTree = layout.GenerateOverworld(true, true);
            worldGen.data.voronoiTree = layout.voronoiTree;
            layout.PopulateSubworlds();
            worldGen.CompleteLayout((_, _, _) => true);

            return worldGen;
        }
    }
}
