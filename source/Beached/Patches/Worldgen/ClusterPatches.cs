using HarmonyLib;
using ProcGen;
using ProcGenGame;

namespace Beached.Patches.Worldgen
{
    public class ClusterPatches
    {

        [HarmonyPatch(typeof(Cluster), "Load")]
        public class Cluster_Load_Patch
        {
            public static void Postfix(Cluster __result)
            {
                foreach(var world in __result.worlds)
                {
                    var test = world.Settings.GetStringSetting("TestValue");
                    Log.Debug($"{world.Settings.mutatedWorldData.world.name}: {test}");

                    /*
                    foreach (var item in data.defaultsOverrides.data)
                    {
                        var value = item.Value ?? "null";
                        Log.Debug($"{item.Key}: {value}");
                    }
                                    }

                                    if(data.defaultsOverrides.data.TryGetValue("TestData", out object testData))
                                    {
                                        var testDictionary = testData as Dictionary<string, object>;
                                        Log.Debug("TEST DATA: " + testDictionary["SomeKey"] as string);
                                    }
                                    else
                                    {
                                        Log.Debug("no test data");
                                    }*/
                }
            }
        }
    }
}
