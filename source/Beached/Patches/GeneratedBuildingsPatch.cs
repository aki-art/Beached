using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Postfix()
			{
				foreach (var buildingDef in Assets.BuildingDefs)
				{
					if (buildingDef.BuildingComplete.TryGetComponent(out Door _))
						ModAPI.ExtendPrefabToLubricatable(buildingDef.BuildingComplete, 10, 10f / 36f, false);
				}

				var planInfo = new PlanScreen.PlanInfo(
					BDb.poisBuildCategory,
					false,
					[
						ForceFieldGeneratorConfig.ID
					]);

				TUNING.BUILDINGS.PLANORDER.Add(planInfo);

				Recipes.AddRecipes();

				var stirlingEngine = Assets.GetBuildingDef("StirlingEngine");
				if (stirlingEngine != null)
					ModAPI.ExtendPrefabToLubricatable(stirlingEngine.BuildingComplete, 10f, 10f / CONSTS.CYCLE_LENGTH, true);
			}
		}
	}
}
