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
				var planInfo = new PlanScreen.PlanInfo(
					BDb.poisBuildCategory,
					false,
					[
						ForceFieldGeneratorConfig.ID
					]);

				TUNING.BUILDINGS.PLANORDER.Add(planInfo);

				Recipes.AddRecipes();

				ConfigureLubricatableBuildingPrefabs();
			}

			private static void ConfigureLubricatableBuildingPrefabs()
			{
				AddTimerLubricatable(HydrogenGeneratorConfig.ID);
				AddTimerLubricatable(ManualGeneratorConfig.ID);
				AddTimerLubricatable("StirlingEngine");

				AddLubricatable(RockCrusherConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(SludgePressConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(OreScrubberConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(LiquidPumpingStationConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(MilkPressConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(DiamondPressConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(UraniumCentrifugeConfig.ID, ModTuning.standardLubricantUses);

				foreach (var buildingDef in Assets.BuildingDefs)
				{
					if (buildingDef.BuildingComplete.TryGetComponent(out Door _))
						AddLubricatable(buildingDef.PrefabID, ModTuning.standardLubricantUses);
				}
			}

			private static void AddTimerLubricatable(string prefabId, float time = CONSTS.CYCLE_LENGTH)
			{
				AddLubricatable(prefabId, 10f, 10f / time, true);
			}

			private static void AddLubricatable(string prefabId, int times)
			{
				AddLubricatable(prefabId, 10f, 10f / times, false);
			}

			private static void AddLubricatable(string prefabId, float capacity, float kgUsedEachTime, bool isTimedUse)
			{
				var def = Assets.GetBuildingDef(prefabId);
				if (def == null)
					return;

				ModAPI.ExtendPrefabToLubricatable(def.BuildingComplete, capacity, kgUsedEachTime, isTimedUse);
			}
		}
	}
}
