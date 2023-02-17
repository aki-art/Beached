using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                foreach (var buildingDef in Assets.BuildingDefs)
                {
                    if (buildingDef.BuildingComplete.TryGetComponent(out Door _))
                    {
                        Lubricatable.ConfigurePrefab(buildingDef.BuildingComplete, 10, 10f / 36f);
                    }
                }
            }
        }
    }
}
